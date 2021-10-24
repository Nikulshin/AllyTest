//import {FormControl, InputGroup, Button} from "react-bootstrap";
import React from "react";
import DatePicker from "react-datepicker";

export class InputsComponent extends React.Component {

    constructor (props) {
        super(props);
        this.state = {valuationDate: new Date()};
        this.calculate = this.calculate.bind(this);
    }

    calculate() {
        this.props.calculate(this.state.valuationDate);
    }


    render() {
        return (
            <>
                <DatePicker selected={this.state.valuationDate} onChange={(date) => this.setState({valuationDate: date})}/>
                <button onClick={this.calculate}>Calculate</button>
                {/*<div>{this.state.valuationDate.toDateString()}</div>*/}
            </>
        );
    }
}
