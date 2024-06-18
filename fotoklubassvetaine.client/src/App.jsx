import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginForm from './components/LoginForm.jsx';
import MainForm from './components/MainForm.jsx';
import PhotoPreview from './components/PhotoPreview.jsx';
import UploadPhoto from './components/UploadPhoto.jsx';
import PersonalInfo from './components/PersonalInfo.jsx';
import CreateAccount from './components/CreateAccount.jsx';
import LoginInformation from './components/LoginInfoForm.jsx';
import PhotoDetails from './components/PhotoDetails.jsx';
import './App.css';

function App() {
    const [user, setUser] = useState({ username: '', password: '' });

    const handleLogin = (username, password) => {
        setUser({ username, password });
    };

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<LoginForm onLogin={handleLogin} />} />
                <Route path="/main" element={<MainForm />} />
                <Route path="/photo-preview" element={<PhotoPreview />} />
                <Route path="/upload-photo" element={<UploadPhoto username={user.username} />} />
                <Route path="/personal-info" element={<PersonalInfo username={user.username} />} />
                <Route path="/login-info" element={<LoginInformation username={user.username} password={user.password} />} />
                <Route path="/create-account" element={<CreateAccount />} />
                <Route path="/photo-details" element={<PhotoDetails />} />
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}

export default App;
