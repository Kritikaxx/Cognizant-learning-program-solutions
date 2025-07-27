import React from 'react';
import Post from './Post';

class Posts extends React.Component {
constructor(props) {
    super(props);
    this.state = {
    posts: [],
    hasError: false,
    error: null
    };
}

loadPosts() {
    fetch('https://jsonplaceholder.typicode.com/posts')
    .then(response => response.json())
    .then(data => {
        const postList = data.map(item => new Post(item.id, item.title, item.body));
        this.setState({ posts: postList });
    })
    .catch(error => {
        this.setState({ hasError: true, error });
    });
}

componentDidMount() {
    this.loadPosts();
}

componentDidCatch(error, info) {
    alert("An error occurred: " + error);
    this.setState({ hasError: true, error });
}

render() {
    if (this.state.hasError) {
    return <h3 style={{ color: 'red' }}>Something went wrong: {this.state.error.toString()}</h3>;
    }

    return (
    <div>
        <h1>Posts</h1>
        {this.state.posts.map(post => (
        <div key={post.id} style={{ marginBottom: '20px' }}>
            <h2>{post.title}</h2>
            <p>{post.body}</p>
        </div>
        ))}
    </div>
    );
}
}

export default Posts;
