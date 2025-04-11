import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import './css/PersonalInfo.css';

const PersonalInfo = ({ username }) => {
    const [userInfo, setUserInfo] = useState(null);

    useEffect(() => {
        if (username) {
            const fetchUserInfo = async () => {
                try {
                    const response = await fetch(`https://localhost:7001/user/userinfo?username=${username}`);
                    if (response.ok) {
                        const data = await response.json();
                        console.log('Fetched user info:', data);
                        setUserInfo(data);
                    } else {
                        console.error('Failed to fetch user info');
                    }
                } catch (error) {
                    console.error('Error fetching user info:', error);
                }
            };
            fetchUserInfo();
        }
    }, [username]);

    return (
        <div className="PersonalInfo">
            <h2>Personal Information</h2>
            {userInfo ? (
                <>
                    <p>Vardas: {userInfo.vardas}</p>
                    <p>Pavarde: {userInfo.pavarde}</p>
                    <p>Elpas: {userInfo.elpas}</p>
                    <p>TelNR: {userInfo.telNR}</p>
                    <p>Naryste: {userInfo.naryste}</p>
                    <p>Prisijungimo DAT: {new Date(userInfo.prisijungimoDAT).toLocaleDateString()}</p>
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
