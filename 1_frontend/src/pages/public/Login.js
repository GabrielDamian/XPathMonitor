import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { config } from "../../config";
import Cookies from "js-cookie";
import "./style/Login.css";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";

export default function Login() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async () => {
    try {
      //test
      let response = await fetch(`${config.server}/auth/login`, {
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

  const styles = {
    style: { backgroundColor: "var(--primary)", color: "red" },
    InputLabelProps: {
      style: { color: "white", fontWeight: "bold" },
    },
    InputProps: {
      style: { color: "gray" },
    },
  };
  return (
    <div className="landing-container">
      <div className="landing-top">
        <div className="landing-top-left">
          <a href="/">Price tracker</a>
        </div>
        <div className="landing-top-right">
          <a href="/login" className="landing-top-right-login">
            Login
          </a>
          <a href="/signup" className="landing-top-right-register">
            Register
          </a>
        </div>
      </div>
      <div className="login-core">
        <div className="login-form">
          <TextField
            label="Username"
            type="text"
            name="username"
            onChange={handleChange}
            value={username}
            variant="outlined"
            {...styles}
          />

          <TextField
            label="Password"
            type="password"
            name="password"
            onChange={handleChange}
            value={password}
            variant="outlined"
            sx={{
              marginTop: "30px",
            }}
            style={{ backgroundColor: "var(--primary)", color: "red" }}
            {...styles}
          />
          <Button
            sx={{
              backgroundColor: "var(--secondary)",
              marginTop: "30px",
            }}
            variant="contained"
            onClick={handleSubmit}
          >
            Login
          </Button>
        </div>
      </div>
    </div>
  );
}
