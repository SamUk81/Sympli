import logo from './logo.svg';
import './App.css';
import SearchPage from './Component/Search';

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo" />
                <SearchPage />
            </header>
        </div>
    );
}

export default App;
