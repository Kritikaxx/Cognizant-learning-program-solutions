import CohortDetails from './Components/CohortDetails';

function App() {
  return (
    <div>
      <CohortDetails
        name="React Bootcamp"
        startDate="2025-06-01"
        endDate="2025-07-30"
        status="ongoing"
      />
      <CohortDetails
        name="Java Fundamentals"
        startDate="2025-04-01"
        endDate="2025-05-15"
        status="completed"
      />
      <CohortDetails
        name="Cognizant DEEP-Skilling"
        startDate="2025-06-19"
        endDate="2025-07-7"
        status="ongoing"
      />
    </div>
  );
}

export default App;
