var appApi = {};
var appUrls = {};

/**
 * Return a Json object from a Html form instance.
 * @param $form
 * @returns Json object.
 */
appApi.getFormDataInJson = function($form) {
    var dataJson = {};
    var dataArray = $form.serializeArray();
    for (var item in dataArray) {
        if (dataArray.hasOwnProperty(item)) {
            // Skip if the property doesn't have a name,
            // Or skip if the property doesn't belong the actual array (Prorotype properties).
            if (dataArray[item].name === undefined || dataArray[item].name === "") {
                continue;
            }
            // Get the property value.
            var fieldValue = dataArray[item].value;
            // Fix the boolean values in string.
            if (fieldValue === "true")
                fieldValue = true;
            else if (fieldValue === "false")
                fieldValue = false;

            if (dataJson.hasOwnProperty(dataArray[item].name) && typeof (fieldValue) === "boolean") {
                continue;
            }

            if (dataJson.hasOwnProperty(dataArray[item].name) && typeof (fieldValue) !== "boolean") {
                if (dataJson[dataArray[item].name].constructor !== Array) {
                    dataJson[dataArray[item].name] = [dataJson[dataArray[item].name]];
                }
                dataJson[dataArray[item].name].push(fieldValue);
            } else {
                dataJson[dataArray[item].name] = fieldValue;
            }
        }
    }
    return dataJson;
};

/**
    * Call an Api URL and returns JSON values.
    * @param url
    * @param onSuccess
    * @param onError
    * @param onComplete
    * @returns void
    */
appApi.getJson = function(url, onSuccess, onError, onComplete) {
    if (onError == null)
        onError = function(exception) { alertify.alert(exception.responseJSON.ErrorMessage); };
    var axajCallOptions = {
        type: "GET",
        url: url,
        datatype: "json",
        traditional: true,
        success: onSuccess,
        error: onError,
        complete: onComplete
    };
    $.ajax(axajCallOptions);
};

/**
 * Post a form values to an action method using Json.
 * @param url
 * @param postData
 * @param onSuccess
 * @param onError
 * @param onComplete
 * @returns void
 */
appApi.postForm = function(url, postData, onSuccess, onError, onComplete) {
    if (onError == null)
        onError = function(exception) { alertify.alert(exception.responseJSON.ErrorMessage); };
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        cache: false,
        data: JSON.stringify(postData),
        success: onSuccess,
        error: onError,
        complete: onComplete
    });
};

/**
 * Post a file value to an action method.
 * @param url
 * @param postData
 * @param onSuccess
 * @param onError
 * @param onComplete
 * @returns void
 */
appApi.postFile = function (url, postData, onSuccess, onError) {
    if (onError == null)
        onError = function () { alertify.alert("Exception occured in the system."); };
    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            onSuccess(JSON.parse(xhr.responseText));
        } else {
            onError();
        }
    }
    xhr.send(postData);
};

// ##### Alertify Module Settings #####
alertify.defaults.transition = "zoom";
alertify.defaults.maximizable = false;
alertify.defaults.theme.ok = "btn btn-primary";
alertify.defaults.theme.cancel = "btn btn-danger";
alertify.defaults.theme.input = "form-control";
alertify.defaults.glossary.title = "People Search Application";

// Override jQuery.Validate plugin defaults.
$.validator.setDefaults({
    highlight: function(element) {
        $(element).closest(".form-group").addClass("has-error");
    },
    unhighlight: function(element) {
        $(element).closest(".form-group").removeClass("has-error").addClass("has-success");
    },
    errorElement: "span",
    errorClass: "help-block",
    errorPlacement: function(error, element) {
        if (element.parent(".input-group").length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});

// Add Bootstrap error class to all invalid controls.
$(function() {
    $("form").each(function() {
        $(this).find("div.form-group").each(function() {
            if ($(this).find("span.field-validation-error").length > 0) {
                $(this).addClass("has-error");
            }
        });
    });
});

// Raise jquery.unobtrusive.validator on ajax calls.
$(document).ajaxComplete(function() {
    /**
     * Re-parse the DOM after Ajax to enable client validation
     * for any new form fields that have it enabled.
     */
    $.validator.unobtrusive.parse(document);
});