import {FormControl, InputGroup} from "react-bootstrap";

export function InputsComponent (props) {
    return (
        <>
            <InputGroup className="mb-3">
                <InputGroup.Text id="basic-addon1">@</InputGroup.Text>
                <FormControl
                    placeholder="Username"
                    aria-label="Username"
                    aria-describedby="basic-addon1"
                />
            </InputGroup>
        </>
    );
}
