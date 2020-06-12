var ActiveOnlyWidget = (function () {

	var self = null;

	function ActiveOnlyWidget(input_el) {

		self = this;

		$(input_el).click(function ()
		{            
            var search_path = input_el.getAttribute('data-search-url');

            if (search_path != null) {
                location.href = search_path + '?ActiveOnly=' + input_el.checked;
            }
        });		
	};

	return ActiveOnlyWidget;

})();

function InitializeActiveOnlyWidgets() {
    
    var inputs = document.getElementsByClassName('active_only_checker');

	for (var i = 0; i < inputs.length; i++) {
	    var widget = new ActiveOnlyWidget(inputs[i]);
	}
}

