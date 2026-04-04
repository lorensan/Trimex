import React from 'react';

const Section = ({ id, title, children, className = "" }) => {
    return (
        <section id={id} className={`section ${className}`}>
            {title && <h2>{title}</h2>}
            <div className="section-content">
                {children}
            </div>
        </section>
    );
};

export default Section;
