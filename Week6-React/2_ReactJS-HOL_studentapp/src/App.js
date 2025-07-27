import './App.css';
import About from './components/About';
import Contact from './components/Contact';
import Home from './components/Home';


function App() {
  return (
    <center><div className="container">
      <Home />
      <About />
      <Contact />
    </div></center>
  );
}

export default App;
