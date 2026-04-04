import React from 'react';

const Sidebar = () => {
    const items = [
        { icon: 'grid_view', label: 'Home', target: 'home' },
        { icon: 'fitness_center', label: 'About', target: 'about' },
        { icon: 'monitoring', label: 'Process', target: 'how-it-works' },
        { icon: 'settings', label: 'Contact', target: 'contact' }
    ];

    const scrollTo = (id) => {
        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth' });
        }
    };

    return (
        <aside className="sidebar">
            <div className="sidebar-logo" style={{ marginBottom: '3rem' }}>
                <span className="material-symbols-outlined" style={{ color: 'var(--accent-primary)', fontSize: '2.5rem' }}>bolt</span>
            </div>
            {items.map((item, index) => (
                <div key={index} className="sidebar-icon" onClick={() => scrollTo(item.target)}>
                    <span className="material-symbols-outlined">{item.icon}</span>
                    <span className="label">{item.label}</span>
                </div>
            ))}
        </aside>
    );
};

export default Sidebar;
