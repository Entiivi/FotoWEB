import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate} from 'react-router-dom';
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
    // Create a state to store logged-in user info
    const [user, setUser] = useState(null);

    // Update the login handler to store the username and user id (narysID)
    const handleLogin = (username, narysID) => {
        console.log('User logged in:', username);
        setUser({ username, narysID });
    };

    return (
        <Router>
            <div className="app-container">
                <Routes>
                    {/* Pass the required onLogin prop to LoginForm */}
                    <Route path="/login" element={<LoginForm onLogin={handleLogin} />} />
                    <Route path="/main" element={<MainForm />} />
                    <Route path="/photo-preview" element={<PhotoPreview />} />
                    <Route path="/upload-photo" element={<UploadPhoto username = { user? user.username : ''} />}/>
                    {/* Pass the username as a prop to PersonalInfo */}
                    <Route path="/personal-info" element={<PersonalInfo username={user ? user.username : ''} />} />
                    <Route
                        path="/login-info"
                        element={
                            <LoginInformation
                                userId={user ? user.narysID : ''}
                                username={user ? user.username : ''}
                            />
                        }
                    />
                    <Route path="/create-account" element={<CreateAccount />} />
                    <Route path="/photo-details" element={<PhotoDetails />} />
                    <Route path="*" element={<Navigate to="/login" />} />
                </Routes>
            </div>
        </Router>
    )
}

export default App;
