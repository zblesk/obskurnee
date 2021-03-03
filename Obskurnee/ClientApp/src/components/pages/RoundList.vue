<template>
<section>
<h1 id="tableLabel" class="page-title">Kolá návrhov</h1>

<a @click="toggleVisibility">Ukaz/skry</a>
<div v-if="isMod && !hide" class="page">
    <h2 class="form-heading">Založení nového kola</h2>
    <span class="todo-l">toto mozno skor na spodok pod zoznam, nech to nezabera miesto. User sem castejsie bude chodit pozerat nez zakladat nove kola. Update: ako pozeram, mozno ak by sa to zakamuflovalo aby to nebolo moc vyrazne, moze ostat hore... nuz,. necham na Teba, posud ako to bude vyzerat so stylmi zvysku stranky.</span>
    <div class="form-field">
        <span>Začít s </span>
        <input type="radio" id="Books" value="Books" v-model="newRound.topic">
        <label for="Books" class="label-radio">knihami</label>
        <span class="radio-between">nebo</span>
        <input type="radio" id="Themes" value="Themes" v-model="newRound.topic">
        <label for="Themes" class="label-radio">tématy</label>
    </div>
    <div class="form-field">
        <label for="name" class="label">Názov (napr. 'Kniha IXY')</label>
        <input v-model="newRound.title" id="name" required />
    </div>
    <div class="form-field">
        <label for="description" class="label">Popis</label>
        <textarea v-model="newRound.description" id="description"></textarea>
    </div>
    <button @click="cNewRound" class="button-primary">Nové kolo</button>
</div>

<p v-if="!rounds"><em>Cakaj, nacitavam</em></p>
<div v-for="round of rounds" v-bind:key="round" style="margin:auto;"> 
    <h3>{{ round.title }}</h3>
    <table>
        <tr>
            <td><router-link v-if="round.themeDiscussionId" :to="{ name: 'discussion', params: { discussionId: round.themeDiscussionId } }">Návrhy tém</router-link></td>
            <td>
                <router-link v-if="round.themePollId" :to="{ name: 'poll', params: { pollId: round.themePollId } }">Hlasovanie o témach</router-link> 
            </td>
        </tr><tr>
            <td><router-link v-if="round.bookDiscussionId" :to="{ name: 'discussion', params: { discussionId: round.bookDiscussionId } }">Návrhy kníh</router-link> </td>
            <td><router-link v-if="round.bookPollId" :to="{ name: 'poll', params: { pollId: round.bookPollId } }">Hlasovanie o knihách</router-link> </td>
        </tr>
        <tr>
            <td><router-link v-if="round.bookId" :to="{ name: 'book', params: { bookId: round.bookId } }">Kniha</router-link></td>
        </tr>
    </table>
</div>
</section>
</template>


<script>
import { mapActions, mapState, mapGetters } from "vuex";
export default {
    name: "RoundList",
    data() { 
        return {
            newRound: {
                topic: "Books",
            },
            hide: true,
        }
    },
    computed: {
        ...mapState("rounds", ["rounds"]),
        ...mapGetters("context", ["isMod"])
    },
    methods: {
        ...mapActions("rounds", ["fetchRounds", "createNewRound"]),
        cNewRound()
        {
            this.createNewRound(this.newRound).then(() => 
            {
                this.newRound = {};
            });
        },
        toggleVisibility()
        {
            this.hide = !this.hide;
        }
    },
    mounted() {
        this.fetchRounds();
    }
}
</script>

<style scoped>
    .page {
        max-width: 800px;
        background-color: var(--c-bckgr-primary);
        margin: var(--spacer);
        padding: calc(2* var(--spacer));
    }

    @media screen and (min-width: 840px) {
        .page {
            margin: var(--spacer) auto;
        }
    }

    .form-heading {
        font-size: 1.5em;
        padding: 0;
        margin-top: 0;
        margin-bottom: var(--spacer);
    }

    .form-field {
        margin-bottom: var(--spacer);
    }

    .label {
        display: block;
        font-size: 1em;
        margin-bottom: calc(var(--spacer) / 3);
    }

    .label-radio {
        display: inline-block;
    }

    .form-field input[type="radio"] {
        display: inline-block;
        width: auto;
        margin-left: calc(var(--spacer) / 2);
        margin-right: calc(var(--spacer) / 4);
    }

    .radio-between {
        margin-left: calc(var(--spacer) / 2);
        margin-right: calc(var(--spacer) / 4); 
    }

    .form-field input {
        width: 100%;
        border: 0;
        background-color: var(--c-bckgr-secondary);
        padding: 0.5em;
    }

    .form-field textarea {
        font-size: 1em;
        border: 0;
        background-color: var(--c-bckgr-secondary);
        padding: 0.5em 1em;
        width: 100%;
        height: 5em;
    }


.archived a {
    color: hotpink;
}

td {
    border-color: pink;
    padding: 1em;
}
</style>