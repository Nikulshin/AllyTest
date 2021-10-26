import './App.css';
import {InputsComponent} from "./components/Inputs";
import {Container} from "react-bootstrap";
import {AgGridReact} from "ag-grid-react";
import {useState} from "react";

import 'ag-grid-community/dist/styles/ag-grid.css';
import 'ag-grid-community/dist/styles/ag-theme-alpine.css';

function App() {

    const [gridApi, setGridApi] = useState(null);
    //const [gridColumnApi, setGridColumnApi] = useState(null);

    const onGridReady = (params) => {
        setGridApi(params.api);
        //setGridColumnApi(params.columnApi);
    };

    const formatMoney = (params) => {
        return params.value.toLocaleString();
    }

    const formatDate = (params) => {
        let date = params.value;
        let dateParts = date.split('T');
        return dateParts[0];
    }

    const colDefs = [
        {
            field: "name",
            headerName: "Bank"
        },
        {
            field: "calcLimit",
            headerName: "Trading Limit",
            valueFormatter: formatMoney
        },
        {
            field: "valuationDate",
            headerName: "Date",
            valueFormatter: formatDate
        }
    ];

    const urlBase = "http://localhost:5000/TradeLimits?valuationDate=";

    let tradingLimits = [];

    const pullResults = (date) => {
        fetch(urlBase + date,
            {
                headers: {
                    "Accept": "application/json"
                }
            })
            .then(res => {
                res.json().then(r1 => {
                    tradingLimits = r1;
                    //alert(JSON.stringify(tradingLimits[0]));
                    gridApi.setRowData(tradingLimits);
                })
            });
    }

    const invokeCalc = (date) => {
        let isoStr = date.toISOString();
        let dateStr = isoStr.substring(0, isoStr.indexOf('T'));

      fetch(urlBase + dateStr,
          {
              method: "POST",
              headers: {
                  "Accept": "text/plain"
              }
          })
          .then(r => {
              r.json().then(res => {
                  //alert(JSON.stringify(res));
                  pullResults(dateStr);
              });
              //alert(result);
          }).catch(reason => console.error(reason));

    };

    return (
        <div>
            <Container className="p-3" >
                <h1 className="header">Welcome To Bank Ratings Calculator</h1>
                <div className="Inputs">
                    <InputsComponent calculate={invokeCalc}/>
                </div>
            </Container>
            <div className="ag-theme-alpine" style={{height: 400, width: "100%"}}>
                <AgGridReact
                    rowData={tradingLimits}
                    onGridReady={onGridReady}
                    columnDefs={colDefs}
                >
{/*
                    <AgGridColumn field="name" headerName="Bank"></AgGridColumn>
                    <AgGridColumn field="calcLimit" headerName="Trading Limit"></AgGridColumn>
                    <AgGridColumn field="valuationDate" headerName="Date"></AgGridColumn>
*/}
                </AgGridReact>
            </div>
        </div>

    );
}

/*
* {"id":162,"calcLimit":2000000,"bankId":1,"valuationDate":"2021-10-25T00:00:00"}
* */

export default App;
