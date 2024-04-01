import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import axios from "axios";
import { config } from "../config";

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

      const response = await axios.get(`${config.server}prices/${linkId}`, {
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
      await axios.delete(`${config.server}links/${id}`, {
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
    <div style={{ border: "1px solid red" }}>
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
    </div>
  );
}
