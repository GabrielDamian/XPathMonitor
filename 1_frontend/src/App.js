import { BrowserRouter, Routes, Route } from "react-router-dom";
import Landing from "./pages/public/Landing";
import Login from "./pages/public/Login";
import Signup from "./pages/public/Signup";

import Dashboard from "./pages/private/Dashboard";

import ProtectedRoute from "./pages/ProtectedRoute";
import "./App.css";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* PUBLIC */}
        <Route index element={<Landing />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />

        {/* PRIVATE */}
        {/* <Route path="/dashboard" element={<Dashboard />} /> */}
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}
