import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { config } from "../../config";
import "./style/Login.css";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";

export default function Signup() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async () => {
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
            Signup
          </Button>
        </div>
      </div>
    </div>
  );
}
