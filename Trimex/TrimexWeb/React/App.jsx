import React from 'react';
import { motion } from 'framer-motion';
import Sidebar from './Sidebar';
import RoundMenu from './RoundMenu';
import Section from './Section';

const App = () => {
    const fadeIn = {
        hidden: { opacity: 0, y: 20 },
        visible: { opacity: 1, y: 0, transition: { duration: 0.6 } }
    };

    const staggerContainer = {
        hidden: { opacity: 0 },
        visible: {
            opacity: 1,
            transition: {
                staggerChildren: 0.2
            }
        }
    };

    return (
        <div className="app-container">
            <Sidebar />
            
            <main>
                {/* Hero / Home Section */}
                <motion.section 
                    id="home" 
                    className="hero"
                    initial="hidden"
                    whileInView="visible"
                    viewport={{ once: true }}
                    variants={fadeIn}
                >
                    <div className="hero-background"></div>
                    <div className="hero-overlay"></div>
                    
                    <motion.h1 
                        initial={{ scale: 0.9, opacity: 0 }}
                        animate={{ scale: 1, opacity: 1 }}
                        transition={{ duration: 0.8 }}
                    >
                        TRIMEX <span>PERFORMANCE</span> LAB
                    </motion.h1>
                    <motion.p>PUSH BEYOND LIMITS</motion.p>
                    
                    <RoundMenu />
                </motion.section>

                <Section id="about" title={<>THE KINETIC<br/>ADVANTAGE</>}>
                    <motion.div 
                        className="about-grid"
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true }}
                        variants={staggerContainer}
                    >
                        <motion.div className="card" variants={fadeIn}>
                            <h3>
                                <span className="material-symbols-outlined card-icon">biotech</span>
                                Biometric Integration
                            </h3>
                            <p>Real-time physiological tracking synchronized with every rep. We integrate biomechanical data with aggressive training protocols to redefine the boundaries of human potential.</p>
                        </motion.div>
                        <motion.div className="card" variants={fadeIn}>
                            <h3>
                                <span className="material-symbols-outlined card-icon">model_training</span>
                                Adaptive Load Control
                            </h3>
                            <p>Our AI adjusts resistance dynamically based on fatigue signatures. Precision output measurement for explosive power development.</p>
                        </motion.div>
                        <motion.div className="card" variants={fadeIn}>
                            <h3>
                                <span className="material-symbols-outlined card-icon">speed</span>
                                Velocity Profiling
                            </h3>
                            <p>Establishing your baseline with a full-spectrum biometric scan and strength assessment. 99.8% Precision Accuracy In Every Set.</p>
                        </motion.div>
                    </motion.div>
                </Section>

                <Section id="how-it-works" title="The Performance Protocol">
                    <motion.div 
                        className="timeline"
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true }}
                        variants={staggerContainer}
                    >
                        <motion.div className="step" variants={fadeIn}>
                            <div className="step-number">01</div>
                            <div className="step-content">
                                <h3>Calibrate</h3>
                                <p>Establish your baseline with a full-spectrum biometric scan and strength assessment.</p>
                            </div>
                        </motion.div>
                        <motion.div className="step" variants={fadeIn}>
                            <div className="step-number">02</div>
                            <div className="step-content">
                                <h3>Execute</h3>
                                <p>Follow high-intensity protocols generated specifically for your neural profile.</p>
                            </div>
                        </motion.div>
                        <motion.div className="step" variants={fadeIn}>
                            <div className="step-number">03</div>
                            <div className="step-content">
                                <h3>Optimize</h3>
                                <p>Review granular data analytics to fine-tune recovery and future performance.</p>
                            </div>
                        </motion.div>
                    </motion.div>
                </Section>

                <Section id="contact" title="JOIN THE LAB">
                    <motion.div 
                        className="about-grid"
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true }}
                        variants={staggerContainer}
                    >
                        <motion.div className="card" variants={fadeIn}>
                            <p style={{ letterSpacing: '0.2em', textTransform: 'uppercase', fontSize: '0.75rem', marginBottom: '2rem' }}>Reserve your slot for a performance evaluation</p>
                            
                            <form className="contact-form">
                                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
                                    <input type="text" placeholder="ATHLETE NAME" required />
                                    <select required>
                                        <option>HYPERTROPHY</option>
                                        <option>POWERLIFTING</option>
                                        <option>NEURAL DRIVE</option>
                                        <option>REHAB/RESET</option>
                                    </select>
                                </div>
                                <textarea placeholder="DESCRIBE YOUR OBJECTIVE" rows="4" required></textarea>
                                <button type="submit">TRANSMIT REQUEST</button>
                            </form>
                        </motion.div>
                        
                        <motion.div className="card" variants={fadeIn} style={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center', border: '1px solid var(--border-color)', padding: '2rem', borderRadius: '24px' }}>
                            <span className="material-symbols-outlined" style={{ fontSize: '5rem', color: 'var(--accent-primary)', marginBottom: '2rem' }}>bolt</span>
                            <h3>TRIMEX PERFORMANCE</h3>
                            <p style={{ textAlign: 'center' }}>Precision accuracy in every set. Join the elite laboratory for human performance.</p>
                        </motion.div>
                    </motion.div>
                </Section>
            </main>

            <footer>
                <p>&copy; 2026 TRIMEX PERFORMANCE LAB. ALL RIGHTS RESERVED.</p>
            </footer>
        </div>
    );
};

export default App;

