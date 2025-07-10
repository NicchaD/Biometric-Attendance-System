Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web
Imports System.Collections
Imports System.Web.Caching
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Runtime.Caching


''' <summary>
''' This class will serve as a wrapper to all application level Caching related activities.
''' </summary>
Public NotInheritable Class ApplicationCache
    ''' <summary>
    ''' A private ObjectCache object to handle all application level caching activities
    ''' </summary>
    Private Shared cache As ObjectCache = MemoryCache.[Default]

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Caching period
    ''' </summary>
    Public NotInheritable Class CachePeriod
        Private _dCachePeriod As Double
        Private Sub New()
        End Sub
        Private Sub New(dCachePeriod As Double)
            Dim dShortestCachePeriodInSeconds As Double = 3600
            Dim stShortestCachePeriodInSeconds As String

            stShortestCachePeriodInSeconds = Convert.ToString(ConfigurationManager.AppSettings("ShortestCachePeriodInSeconds"))
            If Not String.IsNullOrEmpty(stShortestCachePeriodInSeconds) AndAlso Not String.IsNullOrWhiteSpace(stShortestCachePeriodInSeconds) Then
                Double.TryParse(ConfigurationManager.AppSettings("ShortestCachePeriodInSeconds"), dShortestCachePeriodInSeconds)
            End If

            _dCachePeriod = dShortestCachePeriodInSeconds * dCachePeriod
        End Sub

        ''' <summary>
        ''' Returns the total cache period in seconds
        ''' </summary>
        ''' <returns>Total Cache Period</returns>
        Public Function ToSeconds() As Double
            Return _dCachePeriod
        End Function
        ''' <summary>
        ''' Cache will be invalidated after a Shortest Cache Period specified in configuration elapse, default is 1 hour.
        ''' </summary>
        Public Shared ReadOnly [Short] As New CachePeriod(1)
        'Default 3600 i.e. 1 Hour
        ''' <summary>
        ''' Cache will be invalidated after a Medium Cache Period elapse, default is (Shortest Cache Period specified in configuration)*3 hours.
        ''' </summary>
        Public Shared ReadOnly Medium As New CachePeriod(3)
        'Default 10800 i.e. 3 Hours
        ''' <summary>
        ''' Cache will be invalidated after a Long Cache Period elapse, default is (Shortest Cache Period specified in configuration)*6 hours.
        ''' </summary>
        Public Shared ReadOnly [Long] As New CachePeriod(6)
        'Default 21600 i.e. 6 Hours
        ''' <summary>
        ''' Cache will be invalidated after a ExtraLong Cache Period elapse, default is (Shortest Cache Period specified in configuration)*24 hours.
        ''' </summary>
        Public Shared ReadOnly ExtraLong As New CachePeriod(24)
        'Default 86400 i.e. 24 Hours
    End Class

    ''' <summary>
    ''' A flag to check whether caching to be done or not
    ''' </summary>
    Private Shared ReadOnly Property CacheEnabled() As Boolean
        Get
            Dim stCacheEnable As String = ConfigurationManager.AppSettings("CacheEnabled")
            If Not String.IsNullOrEmpty(stCacheEnable) AndAlso stCacheEnable.Equals("True", StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the specified cache entry from the cache as an object.
    ''' </summary>
    ''' <param name="stKey">String: A unique identifier for cache entry to get</param>
    ''' <returns>The cache entry that is identified by key</returns>
    Public Shared Function GetCacheValue(stKey As String) As Object
        Try
            Return cache.[Get](stKey)
        Catch ex As Exception

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Removes the cache entry from the cache.
    ''' </summary>
    ''' <param name="stKey">String: A unique identifier for cache entry to be removed</param>
    Public Shared Sub RemoveCacheObject(stKey As String)
        Try
            cache.Remove(stKey)
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Clears all application level cache objects
    ''' </summary>
    Public Shared Sub ClearCache()
        Try
            Dim cachedObjects As Dictionary(Of String, Object) = cache.ToDictionary(Function(x) x.Key, Function(x) x.Value)

            For Each keyValue As KeyValuePair(Of String, Object) In cachedObjects
                RemoveCacheObject(keyValue.Key)
            Next
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Inserts a cache entry into the cache.
    ''' </summary>
    ''' <param name="stKey">String: A unique identifier for cache entry to be added</param>
    ''' <param name="oValue">Object: The object to insert.</param>
    Public Shared Sub AddCache(stKey As String, oValue As Object)
        AddCache(stKey, oValue, CachePeriod.[Short])
    End Sub

    ''' <summary>
    ''' Inserts a cache entry into the cache.
    ''' </summary>
    ''' <param name="stKey">String: A unique identifier for cache entry to be added</param>
    ''' <param name="oValue">Object: The object to insert.</param>
    ''' <param name="ObjCachePeriod">Class: The Caching period after which the cache to be invalidated</param>
    Public Shared Sub AddCache(stKey As String, oValue As Object, ObjCachePeriod As CachePeriod)
        AddCache(stKey, oValue, ObjCachePeriod, Nothing)
    End Sub

    ''' <summary>
    ''' Inserts a cache entry into the cache.
    ''' </summary>
    ''' <param name="stKey">String: A unique identifier for cache entry to be added</param>
    ''' <param name="oValue">Object: The object to insert.</param>
    ''' <param name="ObjCachePeriod">Class: The Caching period after which the cache to be invalidated</param>
    ''' <param name="callback">CacheEntryRemovedCallback: A call back function that to be called on cache invalidating</param>
    Public Shared Sub AddCache(stKey As String, oValue As Object, ObjCachePeriod As CachePeriod, callback As CacheEntryRemovedCallback)
        If Not CacheEnabled Then
            Return
        End If

        Dim policy As CacheItemPolicy = Nothing
        Try
            policy = New CacheItemPolicy()
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ObjCachePeriod.ToSeconds())
            If callback Is Nothing Then
                callback = New CacheEntryRemovedCallback(AddressOf CachedItemRemovedCallback)
            End If
            policy.RemovedCallback = callback
            cache.[Set](stKey, oValue, policy)
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' A private function to be added while inserting cache, if no custom CacheEntryRemovedCallback function is specified
    ''' </summary>
    ''' <param name="arguments">CacheEntryRemovedArguments</param>
    Private Shared Sub CachedItemRemovedCallback(arguments As CacheEntryRemovedArguments)
        HelperFunctions.LogMessage("Application Level Cache '" & arguments.CacheItem.Key.ToString() & "' was removed from Cache because '" & arguments.RemovedReason.ToString() & "'")


    End Sub
End Class

