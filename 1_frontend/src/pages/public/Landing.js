import "./style/Landing.css";

export default function Landing() {
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
      <div className="landing-core">
        <div className="landing-core-left">
          <h1>Keep track of your favorite products prices</h1>
          <p>This app was build using AWS arhitecture (EC2, RDS, Lambda, Event Bridge).</p>
          <a href="/signup"> Get Started</a>
        </div>
        <div className="landing-core-right">
          <div className="landing-core-right-core">
            <img src="/landing/hero.jpg" alt="hero" />
          </div>
        </div>
      </div>
    </div>
  );
}
