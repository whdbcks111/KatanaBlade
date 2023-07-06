(function ($) {

	App.Common.init();

    var loadClass = $('body').data('jsload');
	
    if( App[loadClass] != undefined && App[loadClass].init != undefined )
    {
        App[loadClass].init();
    }
})(jQuery)