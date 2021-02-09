var SearchWidget = (function () {

	var self = null;

	function SearchWidget(input_el) {

		self = this;

		$(input_el)
        .keyup(function (e) {
        	if (e.keyCode == 13) {
        		var search_path = input_el.getAttribute('data-search-url');
        		if (search_path != null) {

        		    if (input_el.value != "")
        		    {
        		        location.href = search_path + '?Search=' + encodeURIComponent(input_el.value);
        		    }
        		    else {
        		        location.href = search_path;
        		    }
        		}
        	}
        });

		$(input_el)
        .focus(function () {
        	setTimeout((function (el) {
        		var strLength = el.value.length;
        		return function () {
        			if (el.setSelectionRange !== undefined) {
        				el.setSelectionRange(strLength, strLength);
        			} else {
        				$(el).val(el.value);
        			}
        		}
        	}(this)), 5);
        });
	};

	return SearchWidget;
})();

function InitializeSearchWidgets() {
	var inputs = document.getElementsByClassName('input-search');

	for (var i = 0; i < inputs.length; i++) {
		var widget = new SearchWidget(inputs[i]);
	}
}

