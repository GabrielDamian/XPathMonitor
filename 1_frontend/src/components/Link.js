import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import axios from "axios";
import { config } from "../config";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { LineChart } from "@mui/x-charts/LineChart";

export default function Link({ linkId, createdAt, description, url, fetchData }) {
  const [linksPrices, setLinkPrices] = useState([]);

  useEffect(() => {
    console.log("linksPrices update:", linksPrices);
  }, [linksPrices]);

  const fetchLinkPrices = async () => {
    try {
      const token = Cookies.get("jwt");
      const response = await axios.get(`${config.server}/prices/${linkId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setLinkPrices(response.data);
    } catch (error) {
      console.log("error", error.message);
    }
  };

  useEffect(() => {
    fetchLinkPrices();
  }, []);

  const deleteItem = async (id) => {
    if (window.confirm(`Do you really want to delete this link ? (Id:${id})`)) {
      try {
        const token = Cookies.get("jwt");
        await axios.delete(`${config.server}/links/${id}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        fetchData();
      } catch (error) {
        console.log("error", error.message);
      }
    }
  };

  const websiteLogos = {
    "it-sh.ro": "link_img/it-sh.png",
    "emag.ro": "link_img/emag.png",
  };

  function extractDomainLogo(link) {
    link = link.replace(/(^\w+:|^)\/\//, "");
    link = link.split("/")[0];
    var domain = link.replace("www.", "");
    if (domain in websiteLogos) {
      return websiteLogos[domain];
    } else {
      return "";
    }
  }

  function truncateStringUrl(str) {
    if (str.length > 15) {
      return str.substring(0, 30) + "...";
    } else {
      return str;
    }
  }

  const chartStyle = {
    "& .MuiChartsAxis-left .MuiChartsAxis-tickLabel": {
      strokeWidth: "0.4",
      fill: "white",
    },
    "& .MuiChartsAxis-bottom .MuiChartsAxis-tickLabel": {
      strokeWidth: "0.5",
      fill: "white",
    },
    "& .MuiChartsAxis-bottom .MuiChartsAxis-line": {
      stroke: "white",
      strokeWidth: 0.4,
    },
    "& .MuiChartsAxis-left .MuiChartsAxis-line": {
      stroke: "white",
      strokeWidth: 0.4,
    },
  };

  return (
    <div className="link-container">
      <Accordion
        style={{ backgroundColor: "var(--primary-light)", color: "white", padding: "15px" }}
      >
        <AccordionSummary
          expandIcon={<ExpandMoreIcon style={{ color: "white" }} />}
          aria-controls="panel1-content"
          id="panel1-header"
        >
          <div className="link-header">
            <p>ID:{linkId}</p>
            <img src={extractDomainLogo(url)} />
          </div>
        </AccordionSummary>
        <AccordionDetails>
          <div className="link-core-top">
            <p>
              <b>Created at:</b> {createdAt}
            </p>
            <p>
              <b>Url:</b> {truncateStringUrl(url)}
            </p>
            <p>
              <b>Description:</b> {description}
            </p>
          </div>
          <div className="link-core-mid">
            <LineChart
              xAxis={[
                {
                  data: [1, 2, 3, 5, 8, 10],
                  axisLine: { color: "red" }, // Culoarea liniei axei x
                  tick: { fill: "blue" }, // Culoarea marcajelor axei x
                },
              ]}
              series={[
                {
                  data: [2, 5.5, 2, 8.5, 1.5, 5],
                },
              ]}
              width={500}
              height={300}
              sx={chartStyle}
            />
          </div>
          <div className="link-core-bot">
            <img src="/bin.png" alt="bin" onClick={() => deleteItem(linkId)} />
          </div>
          {/*
          linksPrices.map((price) => (
            <div key={price.id}>
              <p>
                {price.dateAdded} - <b>{price.priceValue}</b> (priceId: {price.priceId}, linkId:{" "}
                {price.linkId} )
              </p>
            </div>
          ))
          */}
        </AccordionDetails>
      </Accordion>
    </div>
  );
}
