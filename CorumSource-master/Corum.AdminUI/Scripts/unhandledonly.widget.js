var UnhandledOnlyWidget = (function () {

	var self = null;

	function UnhandledOnlyWidget(input_el) {

		self = this;

		$(input_el).click(function ()
		{            
            var search_path = input_el.getAttribute('data-search-url');

            if (search_path != null) {
                location.href = search_path + '?UnhandledOnly=' + input_el.checked;
            }
        });		
	};

	return UnhandledOnlyWidget;

})();

function InitializeUnhandledOnlyWidgets() {
    
    var inputs = document.getElementsByClassName('unhandled_only_checker');

	for (var i = 0; i < inputs.length; i++) {
	    var widget = new UnhandledOnlyWidget(inputs[i]);
	}
}

