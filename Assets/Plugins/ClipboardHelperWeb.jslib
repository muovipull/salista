mergeInto(LibraryManager.library, {
  CopyTextToClipboard: function (text) {
    // Luo väliaikainen textarea-elementti
    var textArea = document.createElement("textarea");
    textArea.value = UTF8ToString(text); // Muunna Unityn string JavaScript stringiksi

    // Sijoita elementti sivulle (piilotettuna)
    textArea.style.position = "absolute";
    textArea.style.left = "-9999px"; // Siirrä pois näkyvistä
    document.body.appendChild(textArea);

    // Valitse teksti ja kopioi se
    textArea.select();
    try {
      var successful = document.execCommand("copy");
      var msg = successful ? "successful" : "unsuccessful";
      console.log("Kopiointi " + msg);
    } catch (err) {
      console.error("Kopiointi epäonnistui: ", err);
    }

    // Poista väliaikainen elementti
    document.body.removeChild(textArea);
  }
});

