import './App.css';
import {InputsComponent} from "./components/Inputs";
import {Container} from "react-bootstrap";

function App() {

    const requestListener = () => {
        alert('Done!');
    };
    const invokeCalc = (date) => {
        alert('before!');
      let req = new XMLHttpRequest();
      req.addEventListener("load", requestListener);
      req.open("POST", "https://localhost:5001/TradeLimits?valuationDate=" + date);
      req.send();
    };

    return (
      <Container className="p-3" >
          <h1 className="header">Welcome To Bank Ratings Calculator</h1>
          <div className="Inputs">
              <InputsComponent calculate={invokeCalc}/>
          </div>
      </Container>

    );
}

export default App;
