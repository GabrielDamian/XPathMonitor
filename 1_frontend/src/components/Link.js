import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import axios from "axios";
import { config } from "../config";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

export default function Link({ linkId, createdAt, description, url, fetchData }) {
  const [linksPrices, setLinkPrices] = useState([]);

  useEffect(() => {
    console.log("linkId", linkId);
    console.log("linksPrices", linksPrices);
  }, [linksPrices]);

  const fetchLinkPrices = async () => {
    try {
      const token = Cookies.get("jwt");
      console.log("token:", token);

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
    console.log("deleteItem:", id);
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
  };
  return (
    <div className="link-container">
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1-content"
          id="panel1-header"
        >
          Accordion 1
        </AccordionSummary>
        <AccordionDetails>
          <b>linkId</b>:<span>{linkId}</span>
          <b>createdAt</b>:<span>{createdAt}</span>
          <b>description</b>:<span>{description}</span>
          <b>url</b>:<span>{url}</span>
          <br />
          <h3>Prices</h3>
          {linksPrices.map((price) => (
            <div key={price.id}>
              <p>
                {price.dateAdded} - <b>{price.priceValue}</b> (priceId: {price.priceId}, linkId:{" "}
                {price.linkId} )
              </p>
            </div>
          ))}
          <button onClick={() => deleteItem(linkId)}>Delete</button>
        </AccordionDetails>
      </Accordion>
    </div>
  );
}
