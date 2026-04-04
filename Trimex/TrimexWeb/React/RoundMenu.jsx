import React from 'react';

const RoundMenu = () => {
    const menus = [
        { label: 'EXPLORE', target: 'about', icon: 'explore' },
        { label: 'TRAIN', target: 'how-it-works', icon: 'bolt' },
        { label: 'ANALYTICS', target: 'how-it-works', icon: 'insights' },
        { label: 'COMMUNITY', target: 'contact', icon: 'share' }
    ];

    const scrollTo = (id) => {
        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth' });
        }
    };

    return (
        <div className="nav-buttons">
            {menus.map((menu, index) => (
                <div 
                    key={index} 
                    className="round-button"
                    onClick={() => scrollTo(menu.target)}
                >
                    <span className="material-symbols-outlined">{menu.icon}</span>
                    <span className="btn-label">{menu.label}</span>
                </div>
            ))}
        </div>
    );
};

export default RoundMenu;
