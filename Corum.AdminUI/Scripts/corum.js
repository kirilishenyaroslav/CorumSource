function addClass(o, c) {

    var re = new RegExp("(^|\\s)" + c + "(\\s|$)", "g")
    if (re.test(o.className)) return
    o.className = (o.className + " " + c).replace(/\s+/g, " ").replace(/(^ | $)/g, "")
}


function removeClass(o, c) {
    var re = new RegExp("(^|\\s)" + c + "(\\s|$)", "g")
    o.className = o.className.replace(re, "$1").replace(/\s+/g, " ").replace(/(^ | $)/g, "")
}

function HideDiv(id, isShow) {
    if (isShow) {
        $("#" + id).show();
    }
    else {
        $("#" + id).hide();
    }
}

function OnPageSizeTemplateChange(url) {
    var PageSizeTemplates = document.getElementById("PageSizeTemplates");

    if (PageSizeTemplates != null) {
        var NewPageSize = PageSizeTemplates.value || PageSizeTemplates.options[PageSizeTemplates.selectedIndex].value;
    }
    if (url.indexOf('?') > -1) {
        url = url + "&PageSize=" + NewPageSize;
    } else {
        url = url + "?PageSize=" + NewPageSize;
    }

    location.href = url;
}


function OnPageSizeOrdersReportsChange(url) {
    var PageSizeTemplates = document.getElementById("PageSizeTemplates");

    //общий отчет
    if (PageSizeTemplates != null) {
        var NewPageSize = PageSizeTemplates.value || PageSizeTemplates.options[PageSizeTemplates.selectedIndex].value;
    }
    url = url + "?PageSize=" + NewPageSize;

    var FilterOrderClientId = null;
    if ($("#FilterOrderClientId") != null) {
        FilterOrderClientId = $("#FilterOrderClientId").val();
        if (FilterOrderClientId.length > 0) {
            url = url + "&FilterOrderClientId=" + encodeURIComponent(FilterOrderClientId);
        }
    }

    var UseOrderClientFilter = false;
    if ($("#UseOrderClientFilter") != null) {
        UseOrderClientFilter = $("#UseOrderClientFilter").val();
        url = url + "&UseOrderClientFilter=" + encodeURIComponent(UseOrderClientFilter.toLowerCase());
    }

    var FilterOrderTypeId = null;
    if ($("#FilterOrderTypeId") != null) {
        FilterOrderTypeId = $("#FilterOrderTypeId").val();
        if (FilterOrderTypeId.length > 0) {
            url = url + "&FilterOrderTypeId=" + encodeURIComponent(FilterOrderTypeId);
        }
    }

    var UseOrderTypeFilter = false;
    if ($("#UseOrderTypeFilter") != null) {
        UseOrderTypeFilter = $("#UseOrderTypeFilter").val();
        url = url + "&UseOrderTypeFilter=" + encodeURIComponent(UseOrderTypeFilter.toLowerCase());
    }


    var FilterOrderDateBeg = null;
    if ($("#FilterOrderDateBeg") != null) {
        FilterOrderDateBeg = $("#FilterOrderDateBeg").val();
        if (FilterOrderDateBeg.length > 0) {
            url = url + "&FilterOrderDateBeg=" + encodeURIComponent(FilterOrderDateBeg);
        }
    }

    var FilterOrderDateBegRaw = null;
    if ($("#FilterOrderDateBegRaw") != null) {
        FilterOrderDateBeg = $("#FilterOrderDateBegRaw").val();
        if (FilterOrderDateBegRaw.length > 0) {
            url = url + "&FilterOrderDateBegRaw=" + encodeURIComponent(FilterOrderDateBegRaw);
        }
    }


    var FilterOrderDateEnd = null;
    if ($("#FilterOrderDateEnd") != null) {
        FilterOrderDateEnd = $("#FilterOrderDateEnd").val();
        if (FilterOrderDateEnd.length > 0) {
            url = url + "&FilterOrderDateEnd=" + encodeURIComponent(FilterOrderDateEnd);
        }
    }

    var FilterOrderDateEndRaw = null;
    if ($("#FilterOrderDateEndRaw") != null) {
        FilterOrderDateEndRaw = $("#FilterOrderDateEndRaw").val();
        if (FilterOrderDateEndRaw.length > 0) {
            url = url + "&FilterOrderDateEndRaw=" + encodeURIComponent(FilterOrderDateEndRaw);
        }
    }

    var PageNumber = null;
    if ($("#PageNumber") != null) {
        PageNumber = $("#PageNumber").val();
        if (PageNumber.length > 0) {
            url = url + "&PageNumber=" + encodeURIComponent(PageNumber);
        }
    }

    location.href = url;
}


function OnPageSizeRestsReportsChange(url) {

    var PageSizeTemplates = document.getElementById("PageSizeTemplates");

    var NewPageSize = 10;

    if (PageSizeTemplates != null) {
        NewPageSize = PageSizeTemplates.value || PageSizeTemplates.options[PageSizeTemplates.selectedIndex].value;
    }

    url = url + "?PageSize=" + NewPageSize;

    var CurrentGroupFieldName = "Storage";
    if ($("#CurrentGroupFieldName_") != null) {
        CurrentGroupFieldName = $("#CurrentGroupFieldName_").val();
        if (CurrentGroupFieldName.length > 0) {
            url = url + "&CurrentGroupFieldName=" + encodeURIComponent(CurrentGroupFieldName);
        }
    }

    var FilterStorageId = null;
    if ($("#FilterStorageId_") != null) {
        FilterStorageId = $("#FilterStorageId_").val();
        if (FilterStorageId.length > 0) {
            url = url + "&FilterStorageId=" + encodeURIComponent(FilterStorageId);
        }
    }

    var FilterRecieverPlanId = null;
    if ($("#FilterRecieverPlanId_") != null) {
        FilterRecieverPlanId = $("#FilterRecieverPlanId_").val();
        if (FilterRecieverPlanId.length > 0) {
            url = url + "&FilterRecieverPlanId=" + encodeURIComponent(FilterRecieverPlanId);
        }
    }

    var FilterRecieverFactId = null;
    if ($("#FilterRecieverFactId_") != null) {
        FilterRecieverFactId = $("#FilterRecieverFactId_").val();
        if (FilterRecieverFactId.length > 0) {
            url = url + "&FilterRecieverFactId=" + encodeURIComponent(FilterRecieverFactId);
        }
    }

    var FilterKeeperId = null;
    if ($("#FilterKeeperId_") != null) {
        FilterKeeperId = $("#FilterKeeperId_").val();
        if (FilterKeeperId.length > 0) {
            url = url + "&FilterKeeperId=" + encodeURIComponent(FilterKeeperId);
        }
    }

    var FilterCenterId = null;
    if ($("#FilterCenterId_") != null) {
        FilterCenterId = $("#FilterCenterId_").val();
        if (FilterCenterId.length > 0) {
            url = url + "&FilterCenterId=" + encodeURIComponent(FilterCenterId);
        }
    }

    var FilterProducerId = null;
    if ($("#FilterProducerId_") != null) {
        FilterProducerId = $("#FilterProducerId_").val();
        if (FilterProducerId.length > 0) {
            url = url + "&FilterProducerId=" + encodeURIComponent(FilterProducerId);
        }
    }

    var UseStorageFilter = false;
    if ($("#UseStorageFilter_") != null) {
        UseStorageFilter = $("#UseStorageFilter_").val();
        url = url + "&UseStorageFilter=" + encodeURIComponent(UseStorageFilter.toLowerCase());
    }

    var UseCenterFilter = false;
    if ($("#UseCenterFilter_") != null) {
        UseCenterFilter = $("#UseCenterFilter_").val();
        url = url + "&UseCenterFilter=" + encodeURIComponent(UseCenterFilter.toLowerCase());
    }

    var UseRecieverPlanFilter = false;
    if ($("#UseRecieverPlanFilter_") != null) {
        UseRecieverPlanFilter = $("#UseRecieverPlanFilter_").val();
        url = url + "&UseRecieverPlanFilter=" + encodeURIComponent(UseRecieverPlanFilter.toLowerCase());
    }

    var UseRecieverFactFilter = false;
    if ($("#UseRecieverFactFilter_") != null) {
        UseRecieverFactFilter = $("#UseRecieverFactFilter_").val();
        url = url + "&UseRecieverFactFilter=" + encodeURIComponent(UseRecieverFactFilter.toLowerCase());
    }

    var UseKeeperFilter = false;
    if ($("#UseKeeperFilter_") != null) {
        UseKeeperFilter = $("#UseKeeperFilter_").val();
        url = url + "&UseKeeperFilter=" + encodeURIComponent(UseKeeperFilter.toLowerCase());
    }

    var UseProducerFilter = false;
    if ($("#UseProducerFilter_") != null) {
        UseProducerFilter = $("#UseProducerFilter_").val();
        url = url + "&UseProducerFilter=" + encodeURIComponent(UseProducerFilter.toLowerCase());
    }

    var PriceForEndConsumer = false;
    if ($("#PriceForEndConsumer_") != null) {
        PriceForEndConsumer = $("#PriceForEndConsumer_").val();
        url = url + "&PriceForEndConsumer=" + encodeURIComponent(PriceForEndConsumer.toLowerCase());
    }

    var PriceForFirstReciver = false;
    if ($("#PriceForFirstReciver_") != null) {
        PriceForFirstReciver = $("#PriceForFirstReciver_").val();
        url = url + "&PriceForFirstReciver=" + encodeURIComponent(PriceForFirstReciver.toLowerCase());
    }

    var PlanFullCost = false;
    if ($("#PlanFullCost_") != null) {
        PlanFullCost = $("#PlanFullCost_").val();
        url = url + "&PlanFullCost=" + encodeURIComponent(PlanFullCost.toLowerCase());
    }

    var PlanChangableCost = false;
    if ($("#PlanChangableCost_") != null) {
        PlanChangableCost = $("#PlanChangableCost_").val();
        url = url + "&PlanChangableCost=" + encodeURIComponent(PlanChangableCost.toLowerCase());
    }

    var FactFullCosts = false;
    if ($("#FactFullCosts_") != null) {
        FactFullCosts = $("#FactFullCosts_").val();
        url = url + "&FactFullCosts=" + encodeURIComponent(FactFullCosts.toLowerCase());
    }

    var FactChangableCosts = false;
    if ($("#FactChangableCosts_") != null) {
        FactChangableCosts = $("#FactChangableCosts_").val();
        url = url + "&FactChangableCosts=" + encodeURIComponent(FactChangableCosts.toLowerCase());
    }

    var BalancePrice = false;
    if ($("#BalancePrice_") != null) {
        BalancePrice = $("#BalancePrice_").val();
        url = url + "&BalancePrice=" + encodeURIComponent(BalancePrice.toLowerCase());
    }

    location.href = url;

}


function InitFilterDropDowns() {

    $("input.filter_dropdown")
        .each(function () {

            InitFilterDropDown(this,
                $(this).attr('data-source-url'),
                $(this).attr('data-droplist-placeholder'));
        });
}


function InitFilterDropDown(element, urlRequest, placeholderString) {

    var pageSize = 10;

    $(element).select2({
        placeholder: placeholderString,
        minimumInputLength: 0,
        allowClear: true,
        initSelection: function (el, callback) {

            if ($(el).val() !== "") {

                var selected_text = $(el).attr('data-selected-text');
                if (selected_text === undefined) {
                    selected_text = $(el).val();
                }

                var initObj = { "id": $(el).val(), "text": selected_text };
                callback(initObj);
            }
        },
        ajax: {
            quietMillis: 150,
            url: urlRequest,
            dataType: 'jsonp',
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
            },
            results: function (data, page) {
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        }
    });
}

function InitCascadeDropDown(element, urlRequest, urlData, placeholderString) {
    var pageSize = 10;

    $(element).select2({
        placeholder: placeholderString,
        minimumInputLength: 0,
        allowClear: true,
        initSelection: function (el, callback) {

            if ($(el).val() !== "") {

                var selected_text = $(el).attr('data-selected-text');
                if (selected_text === undefined) {
                    selected_text = $(el).val();
                }

                var initObj = { "id": $(el).val(), "text": selected_text };
                callback(initObj);
            }
        },
        ajax: {
            quietMillis: 150,
            url: urlRequest,
            dataType: 'jsonp',
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term,
                    urlData: urlData
                };
            },
            results: function (data, page) {
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        }
    });
}

function InitCascade1DropDown(element, urlRequest, urlData1, urlData2, placeholderString) {
    var pageSize = 10;

    $(element).select2({
        placeholder: placeholderString,
        minimumInputLength: 0,
        allowClear: true,
        initSelection: function (el, callback) {

            if ($(el).val() !== "") {

                var selected_text = $(el).attr('data-selected-text');
                if (selected_text === undefined) {
                    selected_text = $(el).val();
                }

                var initObj = { "id": $(el).val(), "text": selected_text };
                callback(initObj);
            }
        },
        ajax: {
            quietMillis: 150,
            url: urlRequest,
            dataType: 'jsonp',
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term,
                    urlData1: urlData1,
                    urlData2: urlData2
                };
            },
            results: function (data, page) {
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        }
    });
}


function InitFilterMultipleDropDowns() {

    $("input.filter_dropdown")
         .each(function () {

             InitMultipleDropDown(this,
             $(this).attr('data-source-url'),
             $(this).attr('data-droplist-placeholder'));
         });
}


function InitMultipleDropDown(element, urlRequest, placeholderString) {

    var pageSize = 10;

    $(element).select2({
        placeholder: placeholderString,
        minimumInputLength: 0,
        allowClear: true,
        multiple: true,
        initSelection: function (el, callback) {

            if ($(el).val() !== "") {

                var id = $(el).val();
                var selected_text = $(el).attr('data-selected-text');
                if (selected_text === undefined) {
                    selected_text = $(el).val();
                }

                //обрабатываем мультивыбор
                var id_array = id.split(',');
                var selected_text_array = selected_text.split(',');

                var data = [];
                for (var i = 0; i < id_array.length; i++) {
                    data.push({ "id": id_array[i], "text": selected_text_array[i] });
                }

                callback(data);
            }
        },
        ajax: {
            quietMillis: 150,
            url: urlRequest,
            dataType: 'jsonp',
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
            },
            results: function (data, page) {
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        }
    });
}

function InitFilterElementMultipleDropDowns(element) {   
        InitMultipleDropDown(element,
                 $(element).attr('data-source-url'),
                 $(element).attr('data-droplist-placeholder'));            
    }

var block = function () {
    $.blockUI({
        css: {
            border: "none",
            padding: "0px",
            backgroundColor: "none",
            color: "#fff"
        },
        message: '<div class="spinner"><div class="dot1"></div><div class="dot2"></div></div>'
    });
}

$(document).ready(function () {

    $('a[href!="#"]a[href]a:not(.delete_link)a:not([role="tab"])').click(function () {
        block();
    });
    $(window).on("beforeunload", function () {
        //block();
    });

    configureSearch();

    bootbox.setDefaults({
        locale: "ru",
        show: true,
        backdrop: true,
        closeButton: true,
        animate: true
    });

});


function configureSearch() {

    $('#user_searchphrase')
        .keyup(function (e) {
            clearTimeout($.data(this, 'timer'));

            if (e.keyCode == 13) {
                var existingString = document.getElementById("user_searchphrase").value;
                window.location.href = '/Users/Users?SearchResult=' + encodeURIComponent(existingString);
            }
            else
                $(this).data('timer', setTimeout(function () {
                    var existingString = document.getElementById("user_searchphrase").value;
                    window.location.href = '/Users/Users?SearchResult=' + encodeURIComponent(existingString);
                }, 500));
        });

    $('#role_searchphrase')
        .keyup(function (e) {
            clearTimeout($.data(this, 'timer'));

            if (e.keyCode == 13) {
                var existingString = document.getElementById("role_searchphrase").value;
                window.location.href = '/Admin/Roles?SearchResult=' + encodeURIComponent(existingString);
            }
            else
                $(this).data('timer', setTimeout(function () {
                    var existingString = document.getElementById("role_searchphrase").value;
                    window.location.href = '/Admin/Roles?SearchResult=' + encodeURIComponent(existingString);
                }, 500));
        });



    $('.input-search')
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

    var focus_element = $('.input-search')[0];
    if (focus_element != null) { focus_element.focus() };

    return;
}

function FullExpandById(treeGrid_id, id) {
    var nodes = [];

    //find the root    
    var row = $('#' + treeGrid_id).jqxTreeGrid('getRow', id);
    while (row != null) {
        if ((row.leaf != true) || (row.leaf == undefined)) {
            nodes.push(row.uid);
        }
        row = row.parent;
    }
    //expand by the chain
    for (var i = nodes.length - 1; i >= 0 ; i--) {
        $('#' + treeGrid_id).jqxTreeGrid('expandRow', nodes[i]);
    }

}

var getNewDate = function (dateIn, timeIn) {
    var day = dateIn.substring(0, 2);
    var month = dateIn.substring(3, 5);
    var year = dateIn.substring(6, 10);
    var hours = timeIn.substring(0, 2);
    var minutes = timeIn.substring(3, 15);
    var newDateOut = new Date(year, month, day, hours, minutes);
    return newDateOut;
}


var gethoursDifference = function (date1, date2) {
    var diff = date2 - date1;
    var hours = Math.floor(diff / (1000 * 60 * 60));
    var hoursStr = hours.toString();
    if (hoursStr.length < 2) {
        hoursStr = '0' + hoursStr;
    }

    var minutes = Math.floor((diff - hours * 60 * 60 * 1000) / (1000 * 60));
    var minutesStr = minutes.toString();
    if (minutesStr.length < 2) {
        minutesStr = '0' + minutesStr;
    }
    var timeStr = hoursStr + ':' + minutesStr + ':00';

    return timeStr;
}

function calculTime(isReturn) {
    var dateFinish = document.querySelector('#FinishDateTimeOfTrip');
    var FinishDateTimeExOfTrip = document.querySelector('#FinishDateTimeExOfTrip');
    var dateReturn = document.querySelector('#ReturnStartDateTimeOfTrip');
    var timeReturn = document.querySelector('#ReturnStartDateTimeExOfTrip');

    var timeWait = document.querySelector('#ReturnWaitingTime');

    if (dateFinish.value != '' && FinishDateTimeExOfTrip.value != '') {
        var newDateFinish = getNewDate(dateFinish.value, FinishDateTimeExOfTrip.value);
    }

    if (dateReturn.value != '' && timeReturn.value != '') {
        var newDateReturn = getNewDate(dateReturn.value, timeReturn.value);
    }

    var isFullData = function () {
        if (dateFinish.value != '' && FinishDateTimeExOfTrip.value != '' && dateReturn.value != '' && timeReturn.value != '')
            return true;
    };

    dateFinish.onchange = function () {
        newDateFinish = getNewDate(dateFinish.value, FinishDateTimeExOfTrip.value);
        if (isFullData() && isReturn) {
            timeWait.value = gethoursDifference(newDateFinish, newDateReturn);
        }
    }

    FinishDateTimeExOfTrip.onchange = function () {
        newDateFinish = getNewDate(dateFinish.value, FinishDateTimeExOfTrip.value);
        if (isFullData() && isReturn) {
            timeWait.value = gethoursDifference(newDateFinish, newDateReturn);
        }
    }

    dateReturn.onchange = function () {
        newDateReturn = getNewDate(dateReturn.value, timeReturn.value);
        if (isFullData() && isReturn) {
            timeWait.value = gethoursDifference(newDateFinish, newDateReturn);
        }
    }

    timeReturn.onchange = function () {
        newDateReturn = getNewDate(dateReturn.value, timeReturn.value);
        if (isFullData() && isReturn) {
            timeWait.value = gethoursDifference(newDateFinish, newDateReturn);
        }
    }


}


function traverseTreeGrid(treeGridId, action) {
    var treeGrid = "$(\"#" + treeGridId + "\")";
    var rows = eval(treeGrid).jqxTreeGrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].records) {
            if (action == "expand") {
                eval(treeGrid).jqxTreeGrid('expandRow', rows[i].rowId);
            } else if (action == "collapse") {
                eval(treeGrid).jqxTreeGrid('collapseRow', rows[i].rowId);
            }
            traverseTree(rows[i].records);
        }
    }
};

function InitTimeRaw() {
    var NewTime = function (time) {
        var hours = time.substring(0, 2);
        var minutes = time.substring(3, 5);
        var today = new Date();
        var dd = today.getDate();
        if (dd < 9) {
            dd = '0' + dd;
        };
        var mm = today.getMonth() + 1;
        if (mm < 9) {
            mm = '0' + mm;
        };
        var yyyy = today.getFullYear();
        var fullDate = yyyy + '-' + mm + '-' + dd + 'T' + hours + ':' + minutes + ':00';
        return fullDate;
    };

    var TimeChange = document.querySelectorAll('.timechange');

    for (var i = 0; i < TimeChange.length; i++) {
        TimeChange[i].addEventListener("change", function () {
            var TimeName = this.getAttribute("name");
            var TimeEl = document.querySelector('#' + TimeName);
            var TimeNameRaw = TimeEl.getAttribute("data-raw-element");
            var TimeElRaw = document.querySelector('#' + TimeNameRaw);
            TimeElRaw.value = NewTime(TimeEl.value);
            var needReturn = document.querySelector('#NeedReturn');
            if (needReturn != null) {
                calculTime(needReturn.value);
            }
        });
    }
};


//window.setInterval(function () {
//    var urlMsg = $("#urlMsg").val();
//     $.ajax({
//            type: 'GET',
//            url: urlMsg,
//            contentType: 'application/json; charset=utf-8',
//            success: function (CountMsg) {
//                if (CountMsg == 0) {
//                    $('#CountMsg').prop('disabled', true);
//                } else {
//                    $('#CountMsg').prop('disabled', false);
//                    $('#CountMsg').text(CountMsg);
//                }

//            }
//     })

//    }, 1000);

function InitializeOrganizations() {
  var source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'Id', type: 'number' },
                        { name: 'Name', type: 'string' },
                        { name: 'Country', type: 'string' },
                        { name: 'CountryId', type: 'number' },
                        { name: 'City', type: 'string' },
                        { name: 'Address', type: 'string' },
                    ],
                    url: "/Orders/GetOrganizations",
                    id: 'Id'
                };

           var dataAdapter = new $.jqx.dataAdapter(source);

           var getLocalization = function () {
               var localizationobj = {};
               localizationobj.pagergotopagestring = "Перейти к стр.:";
               localizationobj.pagershowrowsstring = "Показать строки:";
               localizationobj.pagerrangestring = " из ";
               return localizationobj;
           }

           $("#jqxgridOrganization").jqxGrid(
            {
                width: 590,
                source: dataAdapter,
                theme: 'arctic',
                pageSize: 5,
                sortable: true,
                filterable: true,
                pageable: true,
                showfilterrow: true,
                columnsresize: true,
                autoheight: true,
                autorowheight: true,
                localization: getLocalization(),
                columns: [
                      { text: 'Наименование', datafield: 'Name', width: 200 },
                      { text: 'Страна', datafield: 'Country', width: 100 },
                      { text: 'Город', datafield: 'City', width: 145 },
                      { text: 'Адрес', datafield: 'Address', width: 145 }
                ]
            });


}

function ShowOrganizationDlg(initFields) {
    $('#Organizations')
                .dialog(
                {
                    dialogClass: "jq-dialog-window-custom  modal_dialog",
                    autoOpen: false,
                    width: 610,
                    top: 25,
                    resizable: false,
                    tags: true,
                    title: "Выбор организации",
                    modal: true,
                    open: function (event, ui) {

                    },
                    close: function (event, ui) {

                        $(this).dialog('destroy');
                    },
                    create: function (event, ui) {
                        $("#disable_all").addClass("disable_all");
                    },
                    beforeClose: function (event, ui) {
                        $("#disable_all").removeClass("disable_all");
                    },
                    buttons:
                    [
                        {
                            text: "Отмена",
                            "class": 'cancel-btn btn btn-outline',
                            click: function () {

                                $(this).dialog("close");

                            }
                        },
                {
                    text: "Выбрать",
                    "class": 'btn btn-primary',
                    click: function () {

                        var getselectedrowindexes = $('#jqxgridOrganization').jqxGrid('getselectedrowindexes');

                        if (getselectedrowindexes.length > 0) {
                                var selectedRowData = $('#jqxgridOrganization').jqxGrid('getrowdata', getselectedrowindexes[0]);
                                                      
                                var org ={
                                _Id : selectedRowData.Id,
                                _Name : selectedRowData.Name,
                                _Country: selectedRowData.Country,
                                _CountryId: selectedRowData.CountryId,
                                _City : selectedRowData.City,
                                _Address: selectedRowData.Address
                                }
                                
                                initFields(org);
                               
                            }

                        $(this).dialog("close");

                    }
                }
            ]
        });

    $('#Organizations').dialog('open');

    return;
}

function InitializeRoutes(org) {
    var source =
           {
               datatype: "json",
               datafields: [
                   { name: 'Id', type: 'number' },
                   { name: 'OrgFromName', type: 'string' },
                   { name: 'OrgFromCountry', type: 'string' },
                   { name: 'OrgFromCity', type: 'string' },
                   { name: 'OrgFromAddress', type: 'string' },
                   { name: 'OrgToName', type: 'string' },
                   { name: 'OrgToCountry', type: 'string' },
                   { name: 'OrgToCity', type: 'string' },
                   { name: 'OrgToAddress', type: 'string' }
               ],
               url: "/Customers/GetRoutes",
               id: 'Id',
               pagenum: 0,
               pagesize: 5,
               pager: function (pagenum, pagesize, oldpagenum) {

               }
           };

    var dataAdapter = new $.jqx.dataAdapter(source,
       {
           formatData: function (data) {
               $.extend(data, {
                   featureClass: "P",
                   style: "full",
                   maxRows: 50,
                   username: "jqwidgets"
               });
               return data;
           }
       });

    var getLocalization = function () {
        var localizationobj = {};
        localizationobj.pagergotopagestring = "Перейти к стр.:";
        localizationobj.pagershowrowsstring = "Показать строки:";
        localizationobj.pagerrangestring = " из ";
        return localizationobj;
    }

    var OrgFromNameColumnFilter = function () {
        var filtergroup = new $.jqx.filter();
        var filter_or_operator = 1;
        var filtervalue = org.NameFrom;
        var filtercondition = 'contains';
        var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);
        return filtergroup;
    }();

    var OrgToNameColumnFilter = function () {
        var filtergroup = new $.jqx.filter();
        var filter_or_operator = 1;
        var filtervalue = org.NameTo;
        var filtercondition = 'contains';
        var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);
        return filtergroup;
    }();

    var CountryFromColumnFilter = function () {
        var filtergroup = new $.jqx.filter();
        var filter_or_operator = 1;
        var filtervalue = org.CountryFrom;
        var filtercondition = 'contains';
        var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);
        return filtergroup;
    }();

    var CountryToColumnFilter = function () {
        var filtergroup = new $.jqx.filter();
        var filter_or_operator = 1;
        var filtervalue = org.CountryTo;
        var filtercondition = 'contains';
        var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);
        return filtergroup;
    }();

    $("#jqxgridRoutes").jqxGrid(
    {
        width: 950,
        source: dataAdapter,
        showfilterrow: true,
        filterable: true,
        sortable: true,
        pageable: true,
        pageSize: 5,
        columnsresize: true,
        autoheight: true,
        autorowheight: true,
        columnsheight: '80px',
        localization: getLocalization(),
        columns: [
            { text: 'Организация<br>отправления', datafield: 'OrgFromName', filter: OrgFromNameColumnFilter, width: 135 },
            { text: 'Страна<br>отправления', datafield: 'OrgFromCountry', filter: CountryFromColumnFilter, width: 100 },
            { text: 'Город<br>отправления', datafield: 'OrgFromCity', width: 90 },
            { text: 'Адрес<br>отправления', datafield: 'OrgFromAddress', width: 150 },
            { text: 'Организация<br>прибытия', datafield: 'OrgToName', filter: OrgToNameColumnFilter, width: 135 },
            { text: 'Страна<br>прибытия', datafield: 'OrgToCountry', filter: CountryToColumnFilter, width: 100 },
            { text: 'Город<br>прибытия', datafield: 'OrgToCity', width: 90 },
            { text: 'Адрес<br>прибытия', datafield: 'OrgToAddress', width: 150 }



        ]
    });
}


function InitializeRoutesOrg(orgFrom, OrgTo) {
    var source =
           {
               datatype: "json",
               datafields: [
                   { name: 'Id', type: 'number' },
                   { name: 'OrgFromName', type: 'string' },
                   { name: 'OrgFromCountry', type: 'string' },
                   { name: 'OrgFromCity', type: 'string' },
                   { name: 'OrgFromAddress', type: 'string' },
                   { name: 'OrgToName', type: 'string' },
                   { name: 'OrgToCountry', type: 'string' },
                   { name: 'OrgToCity', type: 'string' },
                   { name: 'OrgToAddress', type: 'string' },
                   { name: 'RouteDistance', type: 'string' },
                   { name: 'RouteTime', type: 'string' },
                   { name: 'ShortName', type: 'string' },
                   { name: 'RouteTime', type: 'string' },
                   { name: 'RouteDistance', type: 'string' }

               ],
               url: "/Customers/GetRoutes",
              // url: '@Url.Action("GetRoutesByFilterCount", "Customers")?OrgFromId= ' + orgFrom + '&OrgToId= ' + OrgTo,
               id: 'Id',
               pagenum: 0,
               pagesize: 5,
               pager: function (pagenum, pagesize, oldpagenum) {

               }
           };

    var dataAdapter = new $.jqx.dataAdapter(source,
       {
           formatData: function (data) {
               $.extend(data, {
                   featureClass: "P",
                   style: "full",
                   maxRows: 50,
                   username: "jqwidgets"
               });
               return data;
           }
       });

    var getLocalization = function () {
        var localizationobj = {};
        localizationobj.pagergotopagestring = "Перейти к стр.:";
        localizationobj.pagershowrowsstring = "Показать строки:";
        localizationobj.pagerrangestring = " из ";
        return localizationobj;
    }

    $("#jqxgridRoutes").jqxGrid(
      {
          width: 950,
          source: dataAdapter,
          sortable: true,
          showfilterrow: true,
          filterable: true,
          pageable: true,
          pageSize: 5,
          columnsresize: true,
          autoheight: true,
          autorowheight: true,
          columnsheight: '80px',
          localization: getLocalization(),
          columns: [
              { text: 'Организация<br>отправления', datafield: 'OrgFromName', width: 200 },
              { text: 'Организация<br>прибытия', datafield: 'OrgToName', width: 200 },
              { text: 'Краткое<br>название', datafield: 'ShortName', width: 350 },
              { text: 'Время/<br>в пути', datafield: 'RouteTime', width: 100 },
              { text: 'Длина<br>маршрута', datafield: 'RouteDistance', width: 100 }
          ]
      });
}
