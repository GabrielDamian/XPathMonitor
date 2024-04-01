import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { config } from "../../config";
import Cookies from "js-cookie";

export default function Login() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async () => {
    try {
      let response = await fetch(`${config.server}auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });
      if (response.ok) {
        let data = await response.json();
        let token = data.token;
        Cookies.set("jwt", token);
        window.alert("Login successful");
        navigate("/dashboard");
      } else {
        window.alert("Login failed");
      }
    } catch (err) {
      console.log(err);
      window.alert("Login failed");
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
      <h1>Login</h1>
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
      <button onClick={handleSubmit}>Login</button>
      <a href="/signup">Signup</a>
      <a href="/dashboard">Dashboard</a>
    </div>
  );
}
