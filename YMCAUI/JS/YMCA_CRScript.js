//Version 1.0.0 - 2010.09.07
//This file is to be used where ever we are making use of Crystal Report Viewer 12.3.
//There is an error in the JS generated through code because of which the Export functionality was not working.
/*
if (typeof __CRYSTALREPORTVIEWERONSUBMIT12 == 'undefined') {
	//alert('Submit method not initialized');
	var __CRYSTALREPORTVIEWERONSUBMIT12 = __doPostBack;
	__doPostBack = function (t, a) {
		bobj.event.publish('saveViewState');
		__CRYSTALREPORTVIEWERONSUBMIT12(t, a);
	};
	//alert('Submit chained');
} else {
	alert('Submit method initialized');
}

function WebForm_OnSubmit() {
	bobj.event.publish('saveViewState');
	return true;
}
*/
