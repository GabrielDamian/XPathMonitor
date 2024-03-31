import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { config } from "../../config";
import axios from "axios";

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
      console.log("error", error.message);
    }
  };

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

  useEffect(() => {
    fetchData();
  }, []);

  const logout = () => {
    Cookies.remove("jwt");
    window.location.href = "/login";
  };

  return (
    <div>
      <h1>Dashboard</h1>
      <p>Welcome to the dashboard</p>

      <a href="/login">Login</a>
      <br></br>
      <a href="/signup">Signup</a>
      <br></br>
      <button onClick={logout}>Logout</button>
      <br></br>
      <div style={{ border: "1px solid black" }}>
        <input
          type="text"
          placeholder="link"
          name="link"
          onChange={handleChange}
          value={newItem["link"]}
        />
        <input
          type="text"
          placeholder="description"
          name="description"
          onChange={handleChange}
          value={newItem["description"]}
        />
        <button onClick={handleAddNewItem}>Add new item</button>
      </div>

      <div style={{ border: "1px solid blue" }}>
        {existingItems.map((item) => (
          <div key={item.id} style={{ border: "1px solid red" }}>
            <b>linkId</b>:<span>{item.linkId}</span>
            <b>createdAt</b>:<span>{item.createdAt}</span>
            <b>description</b>:<span>{item.description}</span>
            <b>url</b>:<span>{item.url}</span>
            <br />
            <button onClick={() => deleteItem(item.linkId)}>Delete</button>
          </div>
        ))}
      </div>
    </div>
  );
}
