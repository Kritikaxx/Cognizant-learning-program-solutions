import CalculateScore from './Components/CalculateScore';

function App() {
  return (
    <div>
      <CalculateScore 
        Name="DhruvKushwaha" 
        School="MJRP Public School" 
        total={298} 
        goal={3} 
      />
    </div>
  );
}

export default App;
