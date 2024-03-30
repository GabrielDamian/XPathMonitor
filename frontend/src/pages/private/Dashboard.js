import { useState, useEffect } from "react";

export default function Dashboard() {
  const [newItem, setNewItem] = useState({ link: "", description: "", xpath: "" });

  const handleChange = (e) => {
    setNewItem({ ...newItem, [e.target.name]: e.target.value });
  };

  const handleAddNewItem = () => {
    console.log("newItem", newItem);
    try {
      fetch("http://localhost:3001/api/items", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify(newItem),
      });
    } catch (error) {
      console.error("Add new item failed", error);
    }
  };

  const [existingItems, setExistingItems] = useState([]);

  useEffect(() => {
    const fetchItems = async () => {
      try {
        const response = await fetch("http://localhost:3001/api/items", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        });
        if (response.ok) {
          const data = await response.json();
          setExistingItems(data);
        }
      } catch (error) {
        console.error("Fetch items failed", error);
      }
    };
    fetchItems();
  }, []);

  return (
    <div>
      <h1>Dashboard</h1>
      <p>Welcome to the dashboard</p>

      <a href="/login">Login</a>
      <br></br>
      <a href="/signup">Signup</a>
      <br></br>

      <div style={{ border: "1px solid black" }}>
        <input type="text" placeholder="link" onChange={handleChange} value={newItem["link"]} />
        <input
          type="text"
          placeholder="description"
          onChange={handleChange}
          value={newItem["description"]}
        />
        <input type="text" placeholder="xpath" onChange={handleChange} value={newItem["xpath"]} />
        <button onClick={handleAddNewItem}>Add new item</button>
      </div>

      <div style={{ border: "1px solid blue" }}>
        {existingItems.map((item) => (
          <div key={item.id} style={{ border: "1px solid red" }}>
            <p>{item.link}</p>
            <p>{item.description}</p>
            <p>{item.xpath}</p>
          </div>
        ))}
      </div>
    </div>
  );
}
