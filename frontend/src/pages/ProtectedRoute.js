import React, { useState, useEffect } from "react";
import Cookies from "js-cookie";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { config } from "../config";

const ProtectedRoute = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const checkToken = async () => {
      const token = Cookies.get("jwt");
      console.log("token from ck:", token);
      if (token) {
        try {
          const response = await axios.get(`${config.server}auth/verify-token`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          if (response.status === 200) {
            setIsAuthenticated(true);
          } else {
            setIsAuthenticated(false);
          }
        } catch (error) {
          setIsAuthenticated(false);
          console.error("Error checking token validity:", error);
        }
      } else {
        setIsAuthenticated(false);
      }
      setIsLoading(false);
    };

    checkToken();
  }, []);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!isAuthenticated) {
    navigate("/login");
    return null;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
