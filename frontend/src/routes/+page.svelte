<script lang="ts">
	import { goto } from '$app/navigation';
</script>

<h1>Welcome to FMS</h1>
<div>
	<button on:click={() => goto('/auth/login')}>Login</button>
	<button on:click={() => goto('/auth/register')}>Register</button>
</div>
