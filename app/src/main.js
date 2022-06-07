// Necessary global variables
var countrySel;
var stateSel;
var newText;
var submitButton;

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

// Functions called on REST API load
function onStatesLoad(stateList) {
    console.log(stateList);

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
            newText.disabled = false;
            submitButton.disabled = false;
        } else {
            // disable adding new states when not selected
            newText.disabled = true;
            submitButton.disabled = true;

            console.log(stateList[this.value].code);
        }
    }
}
function onCountriesLoad(countryList) {
    console.log(countryList);

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
            newText.disabled = false;
            submitButton.disabled = false;
        } else {
            // disable adding new countries when not selected
            newText.disabled = true;
            submitButton.disabled = true;

            console.log(countryList[this.value].code);

            // Populate the list of states and hand control over to onStatesLoad()
            getStates(countryList[this.value].code).then(data => onStatesLoad(data));
        }
    }
}

// Start functions on window load
window.onload = function () {
    // Get a global reference to all of the forms
    countrySel = document.getElementById("country");
    stateSel = document.getElementById("state");
    newText = document.getElementById("newText");
    submitButton = document.getElementById("submit");

    // Populate a list of countries and hand control over to onCountriesLoad()
    getCountries().then(data => onCountriesLoad(data));
}
