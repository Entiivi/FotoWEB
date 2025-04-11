import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import './css/LoginInfoForm.css';

const LoginInformation = ({ userId, username }) => {
    const [password, setPassword] = useState('');

    useEffect(() => {
        const fetchPassword = async () => {
            try {
                const response = await fetch(`https://localhost:7001/user/password?userId=${userId}`);
                if (response.ok) {
                    const data = await response.json();
                    console.log('Fetched password:', data); // Expect data.password
                    setPassword(data.password);
                } else {
                    console.error('Failed to fetch password');
                }
            } catch (error) {
                console.error('Error fetching password:', error);
            }
        };

        if (userId) {
            fetchPassword();
        }
    }, [userId]);

    return (
        <div className="login-info-container">
            <h2>Login Information</h2>
            <p>Username: {username}</p>
            <p>Password: {password}</p>
        </div>
    );
};

LoginInformation.propTypes = {
    userId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
    username: PropTypes.string.isRequired,
};

export default LoginInformation;
