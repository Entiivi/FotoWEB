import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import './css/PersonalInfo.css';

const PersonalInfo = ({ username }) => {
    const [userInfo, setUserInfo] = useState(null); // Initialize as null to check loading state

    useEffect(() => {
        const fetchUserInfo = async () => {
            try {
                const response = await fetch(`https://localhost:7295/user/userinfo?username=${username}`);
                if (response.ok) {
                    const data = await response.json();
                    console.log('Fetched user info:', data); // Debug log
                    setUserInfo(data);
                } else {
                    console.error('Failed to fetch user info');
                }
            } catch (error) {
                console.error('Error fetching user info:', error);
            }
        };

        fetchUserInfo();
    }, [username]);

    return (
        <div className="PersonalInfo">
            <h2>Personal Information</h2>
            {userInfo ? (
                <>
                    <p>Name: {userInfo.vardas}</p>
                    <p>Last Name: {userInfo.pavarde}</p>
                    <p>Email: {userInfo.elpas}</p>
                    <p>Tel: {userInfo.telNR}</p>
                    <p>Todays Date: {new Date().toLocaleDateString()}</p>
                    <p>FotoClub Address: {userInfo.adresas}</p>
                    <p>Membership: {userInfo.naryste}</p>
                </>
            ) : (
                <p>Loading...</p>
            )}
        </div>
    );
};

PersonalInfo.propTypes = {
    username: PropTypes.string.isRequired,
};

export default PersonalInfo;
