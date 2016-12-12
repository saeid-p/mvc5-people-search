var $SearchForm = $("#Search-Form");
var $SearchInput = $("#Search-Input");
var $SearchSpinner = $("#Search-Spinner");
var $SearchResults = $("#Search-Result-Container");
var $PersonCardTemplate = $("#Person-Card-Template");
var $NoResultTemplate = $("#No-Result-Template");
var $ModalContainer = $("#Modal-Container");

var $FormTable = $("#Main-Table").DataTable({
    data: appApi.DataSource,
    columns: [
        { data: "PersonId" },
        { data: "FirstName" },
        { data: "LastName" },
        { data: "BirthDateString" },
        { data: "Address" },
        { data: "PersonId" }
    ],
    columnDefs: [
        {
            targets: 0, // Person Picture
            orderable: false,
            render: function(data, type, row) {
                return "<img src='" + appUrls.GetPersonPictureApi + row.PersonId +
                    "' alt='Person Picture.' width='40' />";
            }
        },
        {
            targets: 5, // Person Interests
            orderable: false,
            render: function(data, type, row) {
                var returnValue = "";
                if (row.Interests !== undefined) {
                    for (var i = 0; i < row.Interests.length; i++) {
                        returnValue += "<span class='badge margin-ex-right'>" + row.Interests[i].Interest + "</span>";
                    }
                }
                return returnValue;
            }
        }
    ],
    order: [[1, "asc"]]
});

// Handles search input submission.
$SearchForm.on("submit", function(e) {
    e.preventDefault();
    var postData = { query: $SearchInput.val() };

    $SearchSpinner.removeClass("hidden");
    $SearchForm.find("button[type='submit']").addClass("disabled");

    appApi.postForm(appUrls.SearchApi, postData, function(postBack) {
        if (postBack.HasError) {
            alertify.alert(postBack.ErrorMessage);
        } else {
            $SearchResults.empty();
            if (postBack.length === 0) {
                $SearchResults.append($NoResultTemplate.text());
            } else {
                displaySearchResults(postBack);
            }
            $SearchResults.removeClass("hidden");
        }
    }, null, function() {
        $SearchSpinner.addClass("hidden");
        $SearchForm.find("button[type='submit']").removeClass("disabled");
    });
});

// Display people info cards based on the search result values.
function displaySearchResults(searchResults) {
    for (var i = 0; i < searchResults.length; i++) {
        var row = searchResults[i];
        var $htmlContent = $($PersonCardTemplate.text()
            .split("{{PersonId}}").join(row.PersonId).split("{{FullName}}").join(row.FullName)
            .split("{{Age}}").join(row.Age === null ? "" : "(" + row.Age + "y)")
            .split("{{Address}}").join(row.Address === null ? "" : row.Address));

        if (row.Interests.length === 0) {
            $htmlContent.find("div.interests").addClass("hidden");
        } else {
            for (var j = 0; j < row.Interests.length; j++) {
                $htmlContent.find("div.interests")
                    .append("<span class='badge margin-ex-right'>" +
                        row.Interests[j].Interest + "</span>");
            }
        }
        $SearchResults.append($htmlContent);
    }
}

// Raised when user submit add modal form.
$("#Add-Person-Button").on("click", function() {
    $ModalContainer.load(appUrls.AddPersonModal, onAddModalLoaded);
});

// Raised when add modal successfully loaded.
var onAddModalLoaded = function() {
    var $modal = getActiveModal();
    $modal.modal();
    $modal.one("shown.bs.modal", onModalShown);
    $modal.one("hidden.bs.modal", function() {
        $ModalContainer.empty(); // Clear modal container child elements.
    });
    getModalForm($modal).on("submit", onAddModalFormSubmit);
};

// Initialize modal default values every time user calls the modal.
var onModalShown = function() {
    $("#FirstName").focus();
    $("#BirthDateContainer").datepicker({ clearBtn: true, autoclose: true, container: "#Active-Modal" });

    appApi.InterestIndex = 0;
    $("#Add-Interest-Button").on("click", onAddInterestButtonClicked);
    $("#Interests-Container").on("click", "[data-role='Interests-Remove']", function(e) {
        $(e.target).parents("[data-role='Interests-item']").remove();
    });
}

// Display a new input to add a new interest.
var onAddInterestButtonClicked = function() {
    var htmlContent = $("#Interest-Input-Template").text()
        .split("{{index}}").join(appApi.InterestIndex.toString());
    appApi.InterestIndex++;
    $("#Interests-Container").append(htmlContent).find("input:last").focus();
}

// Raised when user submit the add modal form.
var onAddModalFormSubmit = function(e) {
    e.preventDefault();
    var $form = $(e.currentTarget);
    if ($form.valid()) {
        var postData = appApi.getFormDataInJson($form);
        appApi.postForm(appUrls.AddPerson, postData, onAddModalFormSubmitted);
    }
};

// Raised when user successfully submitted add modal form.
var onAddModalFormSubmitted = function(postBackData) {
    if (postBackData.HasError) {
        alertify.alert(postBackData.ErrorMessage);
    } else {
        var fileInput = document.getElementById("Picture");
        if (fileInput.files.length > 0) {
            updatePersonPicture(postBackData.NewId, fileInput.files[0]);
        } else {
            onAddModalFormSubmissionCompleted(postBackData.NewId);
        }
    }
};

// Update person picture.
var updatePersonPicture = function(newId, file) {
    var formData = new FormData();
    formData.append("Picture", file);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", appUrls.EditPersonPicture + "/" + newId, true);
    xhr.onreadystatechange = function() {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var picturePostBackData = JSON.parse(xhr.responseText);
            if (picturePostBackData.HasError) {
                alertify.alert(picturePostBackData.ErrorMessage);
            } else {
                onAddModalFormSubmissionCompleted(newId);
            }
        }
    }
    xhr.send(formData);
}

// Raised when modal form submitted successfully.
var onAddModalFormSubmissionCompleted = function(newId) {
    var $modal = getActiveModal();
    var postData = appApi.getFormDataInJson(getModalForm($modal));
    postData.PersonId = parseInt(newId); // Set new generated primary key.
    postData.BirthDateString = postData.BirthDate;
    postData.Interests = updateInterests(postData);

    appApi.DataSource.push(postData); // Update datatable content.
    $FormTable.clear().rows.add(appApi.DataSource).draw();

    $("#Add-Interest-Button").off("click", onAddInterestButtonClicked);
    $("#Interests-Container").off("click", "[data-role='Interests-Remove']");

    getModalForm($modal).off("submit", onAddModalFormSubmit);
    $modal.modal("hide"); // Hide modal.
}

function updateInterests(postData) {
    var interests = [];
    for (var field in postData) {
        if (postData.hasOwnProperty(field)) {
            if (field.substring(0, 10) === "Interests[") {
                interests.push({
                    PersonId: postData.PersonId,
                    Interest: postData[field]
                });
            }
        }
    }
    return interests;
}

function getActiveModal() {
    return $("#Active-Modal");
}

var getModalForm = function($modal) {
    return $modal.find("form");
};