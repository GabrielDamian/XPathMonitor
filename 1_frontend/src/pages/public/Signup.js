import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { config } from "../../config";

export default function Signup() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async () => {
    console.log("test:", config);
    try {
      let response = await fetch(`${config.server}/auth/signup`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });

      if (response.ok) {
        window.alert("Signup successful");
        navigate("/login");
      }
    } catch (err) {
      console.log(err);
      window.alert("Signup failed");
      setUsername("");
      setPassword("");
    }
  };

  const handleChange = (e) => {
    if (e.target.name === "username") {
      setUsername(e.target.value);
    } else if (e.target.name === "password") {
      setPassword(e.target.value);
    }
  };

  return (
    <div>
      <h1>Signup</h1>
      <input
        type="text"
        name="username"
        placeholder="Username"
        onChange={handleChange}
        value={username}
      />
      <input
        type="password"
        name="password"
        placeholder="Password"
        onChange={handleChange}
        value={password}
      />
      <button onClick={handleSubmit}>Signup</button>
      <a href="/login">Login</a>
      <a href="/dashboard">Dashboard</a>
    </div>
  );
}
