import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { config } from "../../config";
import axios from "axios";
import Link from "../../components/Link";
import "./Dashboard.css";
import TextField from "@mui/material/TextField";

export default function Dashboard() {
  const [newItem, setNewItem] = useState({ link: "", description: "" });
  const [existingItems, setExistingItems] = useState([]);

  const handleChange = (e) => {
    setNewItem({ ...newItem, [e.target.name]: e.target.value });
  };

  const fetchData = async () => {
    try {
      const token = Cookies.get("jwt");
      const response = await axios.get(`${config.server}/links`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setExistingItems(response.data);
    } catch (error) {
      console.log("error", error.message);
    }
  };

  const handleAddNewItem = async () => {
    if (!newItem.link || !newItem.description) {
      window.alert("Please fill in all fields");
      return;
    }

    try {
      const token = Cookies.get("jwt");

      await axios.post(
        `${config.server}/links`,
        {
          url: newItem.link,
          description: newItem.description,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      window.alert("New item added successfully");
      setNewItem({ link: "", description: "" });
      fetchData();
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const logout = () => {
    Cookies.remove("jwt");
    window.location.href = "/login";
  };

  return (
    <div className="dash-container">
      <div className="dash-container-top">
        <div className="dash-container-top-left">
          <p>Dashboard</p>
        </div>
        <div className="dash-container-top-right">
          <button onClick={logout}>Logout</button>
        </div>
      </div>
      <div className="dash-container-bot">
        <div className="dash-container-bot-insert">
          <div className="dash-container-bot-insert-col">
            <TextField
              label="Link"
              type="text"
              name="link"
              onChange={handleChange}
              value={newItem["link"]}
            />
          </div>
          <div className="dash-container-bot-insert-col">
            <TextField
              type="text"
              label="description"
              name="description"
              onChange={handleChange}
              value={newItem["description"]}
            />
          </div>
          <div className="dash-container-bot-insert-col">
            <button onClick={handleAddNewItem}>ADD</button>
          </div>
        </div>
        <div className="dash-container-bot-view">
          <h3>History</h3>
          {existingItems.map((item, index) => {
            return <Link key={index} {...item} fetchData={fetchData} />;
          })}
        </div>
      </div>
    </div>
  );
}
