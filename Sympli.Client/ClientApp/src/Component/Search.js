import { Button, TextField, FormLabel } from '@material-ui/core';
import React, { useState, useCallback } from 'react';


const SearchPage = () => {
    const [inputValueSearchKeyword, setInputValueSearchKeyword] = useState("");
    const onChangeHandlerSearchKeyword = event => {
        setInputValueSearchKeyword(event.target.value);
    };

    const [inputValueSearchUrl, setInputValueSearchUrl] = useState("");
    const onChangeHandlerSearchUrl = event => {
        setInputValueSearchUrl(event.target.value);
    };
    const [inputValueChange, setInputValueChange] = useState("");

    const fetchRequest = useCallback(() => {

        async function getData() {
            const apiUrl = `http://localhost:5001/Search?searchKeyword=${inputValueSearchKeyword}&searchUrl=${inputValueSearchUrl}`;
            const res = await fetch(apiUrl);
            const data = await res.text();
            setInputValueChange(data);
        };

        getData();
    }, [inputValueSearchKeyword, inputValueSearchUrl]);

    return (
        <React.Fragment>
            <TextField id="filled-basic" label="SearcKeyword" variant="filled" onChange={onChangeHandlerSearchKeyword}
                value={inputValueSearchKeyword} />
            <TextField id="filled-basic" label="SearchUrl" variant="filled" onChange={onChangeHandlerSearchUrl}
                value={inputValueSearchUrl} />
            <FormLabel>Google Search Position: {inputValueChange}</FormLabel>
            <Button color="primary" onClick={fetchRequest}>Search</Button>
        </React.Fragment>
    )

}
export default SearchPage;

