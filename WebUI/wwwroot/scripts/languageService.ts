function changeLanguage() {
    var language = (document.getElementById("languageSelect") as HTMLInputElement).value;

    if (language === "en" && window.location.href.indexOf("uk") !== -1) {
        window.location.href = window.location.href.replace("uk", language);
    }
    if (language === "uk" && window.location.href.indexOf("en") !== -1) {
        window.location.href = window.location.href.replace("en", language);
    }

    saveLanguageToLocalStorage("language", language);
}

function saveLanguageToLocalStorage(name: string, language: string) {
    localStorage.setItem(name, language);
}

function loadLanguageFromLocalStorage(name: string) {
    const language = localStorage.getItem(name);

    if (language === null) {
        console.log((document.getElementById("languageSelect") as HTMLInputElement).value);
    }
    else {
        (document.getElementById("languageSelect") as HTMLInputElement).value = language;
        localStorage.removeItem(name);
        saveLanguageToLocalStorage(name, (document.getElementById("languageSelect") as HTMLInputElement).value);
    }
}

var langArray: string[] = [];

$("#languageSelect option").each(function () {
    var img = $(this).attr("data-thumbnail");
    var text = this.innerText;
    var value = $(this).val();
    var item = '<li><img src="' + img + '" alt="" value="' + value + '"/><span>' + text + '</span></li>';

    langArray.push(item);
});

$('#a').html(langArray.join(' '));

$('#languageSelectButton').html(generateButtonHtml());
$('#languageSelectButton').attr('value', localStorage.getItem("language"));

$('#a li').click(function () {
    var value = $(this).find('img').attr('value');

    (document.getElementById("languageSelect") as HTMLInputElement).value = value;
    changeLanguage();
});

$("#languageSelectButton").click(function () {
    $("#b").toggle();
});

function generateButtonHtml() {
    let languageCode = window.location.pathname.split("/")[1];

    for (let i = 0; i < langArray.length; i++) {
        if (langArray[i].indexOf('value="' + languageCode + '"') !== -1) {
            return langArray[i];
        }
    }

    return "";
}