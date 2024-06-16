import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PhotoList from './components/PhotoList.jsx';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<PhotoList />} />
            </Routes>
        </Router>
    );
}

export default App;
