import React, { useState } from 'react';
import './css/CreateAccount.css';

function CreateAccount() {
    const [formData, setFormData] = useState({
        Vardas: '',
        Pavarde: '',
        Elpas: '',
        TelNR: '',
        Naryste: '',
        PrisijungimoDAT: '',
        KlubasID: '',
        Slap: '',
        Username: '',
        Administratorius_AdministratoriusID: '',
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevFormData) => {
            const updatedFormData = Object.assign({}, prevFormData);
            updatedFormData[name] = value;
            return updatedFormData;
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('https://localhost:7295/user/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                alert('Account created successfully');
            } else {
                alert('Failed to create account');
            }
        } catch (error) {
            console.error('Error creating account:', error);
        }
    };

    return (
        <div className="create-account">
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Name</label>
                    <input type="text" name="Vardas" value={formData.Vardas} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Last name</label>
                    <input type="text" name="Pavarde" value={formData.Pavarde} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Email</label>
                    <input type="email" name="Elpas" value={formData.Elpas} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Tel</label>
                    <input type="text" name="TelNR" value={formData.TelNR} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Membership</label>
                    <input type="text" name="Naryste" value={formData.Naryste} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Join Date</label>
                    <input type="date" name="PrisijungimoDAT" value={formData.PrisijungimoDAT} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Club ID</label>
                    <input type="text" name="KlubasID" value={formData.KlubasID} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="Slap" value={formData.Slap} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Username</label>
                    <input type="text" name="Username" value={formData.Username} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Administrator ID</label>
                    <input type="text" name="Administratorius_AdministratoriusID" value={formData.Administratorius_AdministratoriusID} onChange={handleChange} />
                </div>
                <div className="actions">
                    <button type="submit" className="create-button">CREATE</button>
                </div>
            </form>
        </div>
    );
}

export default CreateAccount;
