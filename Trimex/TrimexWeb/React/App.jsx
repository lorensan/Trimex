import React, { useRef } from 'react';
import { motion, useScroll, useTransform, AnimatePresence } from 'framer-motion';
import Sidebar from './Sidebar';
import RoundMenu from './RoundMenu';
import Section from './Section';

// Screenshot imports
import amrapImg from '../ScreenShotAPP/Amrap.png';
import customWodImg from '../ScreenShotAPP/CustomWod.png';
import forTimeImg from '../ScreenShotAPP/ForTime.png';
import heroImg from '../ScreenShotAPP/Hero.png';
import mainMenuImg from '../ScreenShotAPP/mainMenu.png';

const App = () => {
    const [selectedView, setSelectedView] = React.useState(null);

    const viewData = {
        amrap: {
            title: "AMRAP",
            content: "Push your limits against the clock. Complete as many rounds as possible within a set time and challenge your endurance, pacing, and mental toughness. Perfect for tracking progress and testing consistency."
        },
        forTime: {
            title: "FOR TIME",
            content: "Race against time and finish the workout as fast as possible. Designed to maximize intensity and performance, this mode rewards efficiency, strategy, and raw determination."
        },
        hero: {
            title: "HERO WOD",
            content: "Honor the legacy. Take on iconic workouts dedicated to fallen heroes, designed to test your physical and mental resilience at the highest level. More than 200 hero wod (male, female and teams)."
        },
        custom: {
            title: "CUSTOM WOD",
            content: "Total flexibility, built your way. Create and tailor your own workouts to match your goals, style, and training needs without limitations."
        },
        mainMenu: {
            title: "TRIMEX HUB",
            content: "The central nervous system of your training. Access all performance protocols, track your evolution, and synchronize your biometrics in one unified interface."
        }
    };

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

    const howItWorksRef = useRef(null);
    const { scrollYProgress } = useScroll({
        target: howItWorksRef,
        offset: ["start end", "end start"]
    });

    const y1 = useTransform(scrollYProgress, [0, 1], [0, -200]);
    const y2 = useTransform(scrollYProgress, [0, 1], [0, -400]);
    const y3 = useTransform(scrollYProgress, [0, 1], [0, -100]);
    const y4 = useTransform(scrollYProgress, [0, 1], [0, -300]);
    const y5 = useTransform(scrollYProgress, [0, 1], [0, -500]);

    const rotate1 = useTransform(scrollYProgress, [0, 1], [-5, 5]);
    const rotate2 = useTransform(scrollYProgress, [0, 1], [5, -5]);
    const rotate3 = useTransform(scrollYProgress, [0, 1], [-10, 10]);
    const rotate4 = useTransform(scrollYProgress, [0, 1], [10, -10]);
    const rotate5 = useTransform(scrollYProgress, [0, 1], [-2, 2]);

    const imageVariants = {
        hover: { 
            scale: 1.1, 
            zIndex: 20, 
            rotate: 0,
            transition: { duration: 0.3, ease: "easeOut" }
        },
        tap: { 
            scale: 0.95,
            transition: { duration: 0.1 }
        }
    };

    return (
        <div className="app-container">
            <div className="static-logo-bg"></div>
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
                    <div className="how-it-works-content" ref={howItWorksRef}>
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

                        <div className="parallax-container">
                            <motion.img 
                                src={heroImg} 
                                alt="Hero" 
                                className="parallax-img img-1" 
                                style={{ y: y1, rotate: rotate1 }} 
                                whileHover="hover"
                                whileTap="tap"
                                variants={imageVariants}
                                onClick={() => setSelectedView('hero')}
                            />
                            <motion.img 
                                src={mainMenuImg} 
                                alt="Main Menu" 
                                className="parallax-img img-2" 
                                style={{ y: y2, rotate: rotate2 }} 
                                whileHover="hover"
                                whileTap="tap"
                                variants={imageVariants}
                                onClick={() => setSelectedView('mainMenu')}
                            />
                            <motion.img 
                                src={amrapImg} 
                                alt="Amrap" 
                                className="parallax-img img-3" 
                                style={{ y: y3, rotate: rotate3 }} 
                                whileHover="hover"
                                whileTap="tap"
                                variants={imageVariants}
                                onClick={() => setSelectedView('amrap')}
                            />
                            <motion.img 
                                src={forTimeImg} 
                                alt="For Time" 
                                className="parallax-img img-4" 
                                style={{ y: y4, rotate: rotate4 }} 
                                whileHover="hover"
                                whileTap="tap"
                                variants={imageVariants}
                                onClick={() => setSelectedView('forTime')}
                            />
                            <motion.img 
                                src={customWodImg} 
                                alt="Custom Wod" 
                                className="parallax-img img-5" 
                                style={{ y: y5, rotate: rotate5 }} 
                                whileHover="hover"
                                whileTap="tap"
                                variants={imageVariants}
                                onClick={() => setSelectedView('custom')}
                            />
                        </div>

                        <AnimatePresence>
                            {selectedView && (
                                <motion.div 
                                    className="view-detail-overlay"
                                    initial={{ opacity: 0, scale: 0.9, y: 20 }}
                                    animate={{ opacity: 1, scale: 1, y: 0 }}
                                    exit={{ opacity: 0, scale: 0.9, y: 20 }}
                                    onClick={() => setSelectedView(null)}
                                >
                                    <div className="detail-box" onClick={(e) => e.stopPropagation()}>
                                        <button className="close-btn" onClick={() => setSelectedView(null)}>
                                            <span className="material-symbols-outlined">close</span>
                                        </button>
                                        <div className="detail-header">
                                            <span className="material-symbols-outlined">info</span>
                                            <h3>{viewData[selectedView].title}</h3>
                                        </div>
                                        <p>{viewData[selectedView].content}</p>
                                        <div className="detail-footer">
                                            <span>TRIMEX PROTOCOL v1.0</span>
                                        </div>
                                    </div>
                                </motion.div>
                            )}
                        </AnimatePresence>
                    </div>
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

