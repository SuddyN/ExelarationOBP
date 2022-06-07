// Necessary global variables
var countrySel;
var stateSel;
var newName;
var newCode;
var submitButton;
var countryList;
var stateList;

// REST API functions
async function getStates(countryCode) {
    let response = await fetch(`https://xc-countries-api.herokuapp.com/api/countries/${countryCode}/states/`);
    let data = await response.json();
    return data;
}
async function getCountries() {
    let response = await fetch("https://xc-countries-api.herokuapp.com/api/countries/");
    let data = await response.json();
    return data;
}
async function postState(entry) {
    let response = await fetch("https://xc-countries-api.herokuapp.com/api/states/", {
        headers: {
            'Content-Type': "application/json"
        },
        method: "POST",
        body: JSON.stringify(entry)
    });
    return response.data;
}
async function postCountry(entry) {
    let response = await fetch("https://xc-countries-api.herokuapp.com/api/countries/", {
        headers: {
            'Content-Type': "application/json"
        },
        method: "POST",
        body: JSON.stringify(entry)
    });
    return response.data;
}

// Functions called on REST API load
function onStatesLoad() {

    // Populate the States dropdown with the stateList
    for (let state in stateList) {
        stateSel.options[stateSel.options.length] = new Option(stateList[state].name, state);
    }
    // Add an option for new states
    stateSel.options[stateSel.options.length] = new Option("MAKE NEW STATE...", "NEW");

    // Do the following whenever a new option is selected...
    stateSel.onchange = function () {
        if (this.value === "NEW") {
            // enable adding new states when selected
            newName.disabled = false;
            newCode.disabled = false;
            submitButton.disabled = false;
        } else {
            // disable adding new states when not selected
            newName.disabled = true;
            newName.value = "";
            newCode.disabled = true;
            newCode.value = "";
            submitButton.disabled = true;
        }
    }
}

function onCountriesLoad() {

    // Populate the Countries dropdown with the countryList
    for (let country in countryList) {
        countrySel.options[countrySel.options.length] = new Option(countryList[country].name, country);
    }
    // Add an option for new countries
    countrySel.options[countrySel.options.length] = new Option("MAKE NEW COUNTRY...", "NEW");

    // Do the following whenever a new option is selected...
    countrySel.onchange = function () {
        // Empty the dropdown list
        stateSel.length = 1;

        if (this.value === "NEW") {
            // enable adding new countries when selected
            newName.disabled = false;
            newCode.disabled = false;
            submitButton.disabled = false;
        } else {
            // disable adding new countries when not selected
            newName.disabled = true;
            newName.value = "";
            newCode.disabled = true;
            newCode.value = "";
            submitButton.disabled = true;

            // Populate the list of states and hand control over to onStatesLoad()
            getStates(countryList[this.value].code).then(data => {
                stateList = data;
                onStatesLoad();
            });
        }
    }
}

// Start functions on window load
window.onload = function () {
    // Get a global reference to all of the forms
    countrySel = document.getElementById("country");
    stateSel = document.getElementById("state");
    newName = document.getElementById("newName");
    newCode = document.getElementById("newCode");
    submitButton = document.getElementById("submit");

    // Create an entry and post depending on selections
    submitButton.onclick = function () {
        if (!newName || !newName.value || !newCode || !newCode.value) return;
        let entry = {
            code: newCode.value.toUpperCase(),
            name: newName.value
        }
        if (countrySel.value === "NEW") {
            postCountry(entry).then(() => {
                getCountries().then(data => {
                    countryList = data;
                    countrySel.value = '';
                    countrySel.length = 1;
                    onCountriesLoad();
                    newName.disabled = true;
                    newName.value = "";
                    newCode.disabled = true;
                    newCode.value = "";
                    submitButton.disabled = true;
                });
            });
        } else if (stateSel.value === "NEW") {
            entry.countryId = countryList[countrySel.value].id;
            postState(entry).then(() => {
                getStates(countryList[countrySel.value].code).then(data => {
                    stateList = data;
                    stateSel.value = '';
                    stateSel.length = 1;
                    onStatesLoad();
                    newName.disabled = true;
                    newName.value = "";
                    newCode.disabled = true;
                    newCode.value = "";
                    submitButton.disabled = true;
                });
            });
        }
    }

    // Populate a list of countries and hand control over to onCountriesLoad()
    getCountries().then(data => {
        countryList = data;
        onCountriesLoad();
    });
}


