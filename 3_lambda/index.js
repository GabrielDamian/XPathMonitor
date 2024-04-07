import fetch from "node-fetch";
import axios from "axios";

const serverUrl = "http://54.173.201.78:5000/Links/special";
const serverUrlPrice = "http://54.173.201.78:5000/prices";

async function main() {
  try {
    const response = await fetch(serverUrl);
    const responseData = await response.json();

    for (let i = 0; i <= responseData.length - 1; i++) {
      let linkId = responseData[i].linkId;
      let url = responseData[i].url;

      let value = 0;
      let valueString = "0";

      try {
        value = await getPageHtmlAndExtractElement2(url);
        valueString = value.toString();
      } catch (err) {
        console.error("Eroare la extragerea pretului:", err.message);
      }

      try {
        await axios.post(serverUrlPrice, {
          linkId: linkId,
          price: valueString,
        });

        console.log("Pret adaugat cu succes!");
      } catch (error) {
        console.error("Eroare, nu se poate adauga un nou pret:", error.message);
      }
    }
  } catch (error) {
    console.error("A apărut o eroare:", error.message);
  }
}

function extractTextAfterMarker(src, marker) {
  // Caută poziția marker-ului în stringul src
  const markerIndex = src.indexOf(marker);

  // Verifică dacă marker-ul a fost găsit
  if (markerIndex === -1) {
    console.error("Markerul nu a fost găsit în stringul src.");
    return null;
  }

  // Extragerea textului de după marker până la primul caracter '&'
  const startIndex = markerIndex + marker.length;

  let endIndex = src.indexOf("&", startIndex);

  // Dacă nu este găsit caracterul '&' după marker, extrage tot textul de după marker
  if (endIndex === -1) {
    endIndex = src.length;
  }

  // Extragerea textului între marker și '&'
  const extractedText = src.substring(startIndex, endIndex);

  return extractedText;
}

async function getPageHtmlAndExtractElement2(url) {
  try {
    const response = await axios.get(url);
    const marker = `<p class="price"><span class="woocommerce-Price-amount amount"><bdi>`;
    const extractedText = extractTextAfterMarker(response.data, marker);
    let number = 0;
    if (extractedText.includes(".")) {
      number = Number(extractedText) * 1000;
    } else {
      number = Number(extractedText);
    }
    return number;
  } catch (error) {
    console.error("A apărut o eroare:", error);
    return null;
  }
}

export const handler = async (event) => {
  try {
    await main();
    return {
      statusCode: 200,
      body: "Execution successful",
    };
  } catch (error) {
    return {
      statusCode: 500,
      body: error.message,
    };
  }
};

async function devMain() {
  let simulateDB = [
    {
      url: "https://www.it-sh.ro/magazin/calculatoare-second-hand/calculatoare-gaming/calculator-gaming-br-19-core-i7-4770-16gb-ddr3-256gb-ssd-1tb-hdd-amd-rx-470-4gb/",
    },
    {
      url: "https://www.it-sh.ro/magazin/calculatoare-second-hand/i3-i5-i7/hp-compaq-8300-elite-sff-i5-3470-3-2ghz8gb-ddr3500gb/",
    },
  ];

  for (let i = 0; i <= simulateDB.length - 1; i++) {
    let url = simulateDB[i].url;

    let value = await getPageHtmlAndExtractElement2(url);
    console.log("Extracted value:", value);
  }
}
