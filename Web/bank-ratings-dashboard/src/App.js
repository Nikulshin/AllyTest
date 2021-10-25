import './App.css';
import {InputsComponent} from "./components/Inputs";
import {Container} from "react-bootstrap";

function App() {

    const invokeCalc = (date) => {
        let isoStr = date.toISOString();
        let dateStr = isoStr.substring(0, isoStr.indexOf('T'));

        let url = "http://localhost:5000/TradeLimits?valuationDate=" + dateStr
        alert(url);

      fetch(url,
          {
              method: "POST",
              headers: {
                  "Accept": "text/plain"
              }
          })
          .then(r => {
              let result = r.json();
              alert(result);
          }).catch(reason => console.error(reason));

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
