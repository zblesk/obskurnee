<template>
<section>
    <h1 id="tableLabel">Aktualne návrhy</h1>

    <p v-if="!discussions"><em>Cakaj, nacitavam</em></p>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="discussions">
        <tbody>
            <tr v-for="discussion of discussions" v-bind:key="discussion"> 
                <td :class="{ archived: discussion.IsClosed }">
                    <router-link :to="{ name: 'discussion', params: { discussionId: discussion.discussionId } }">{{ discussion.title }}</router-link></td>
            </tr>
        </tbody>
    </table>
</section>
</template>


<script>
import { mapActions, mapState } from "vuex";
    export default {
        name: "Discussions",
        data() {
            return {
                newDiscussion: {},
            }
        },
        computed: {
            ...mapState("discussions", ["discussions"]),
        },
        methods: {
            ...mapActions("discussions", ["fetchDiscussionList"]),
        },
        mounted() {
            this.fetchDiscussionList();
        }
    }
</script>

<style scoped>
.archived a {
    color: hotpink;
}
</style>