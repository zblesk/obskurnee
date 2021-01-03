<template>
    <h1 id="tableLabel">{{ title }}</h1>

    <p v-if="!title"><em>Cakaj, nacitavam</em></p>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="title">
        <tbody>
            <tr v-for="post of posts" v-bind:key="post">
                <td>{{ post.text }}</td>
            </tr>
        </tbody>
    </table>
</template>


<script>
    import axios from 'axios'
    export default {
        name: "Discussion",
        data() {
            return {
                posts: [],
                title: ""
            }
        },
        methods: {
            getDiscussionData() {
                axios.get('/api/discussions/' + this.$route.params.id + '/posts')
                    .then((response) => {
                        this.posts =  response.data.posts;
                        this.title = response.data.discussion.title;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        mounted() {
            this.getDiscussionData();
        }
    }
</script>