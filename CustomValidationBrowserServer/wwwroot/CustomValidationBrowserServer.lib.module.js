export function afterWebStarted(blazor) {
    blazor.formValidation.addValidator('wordcount', context => {
        if (!context.value || !context.value.trim()) {
            return { success: true };
        }

        const wordCount = context.value.trim().split(/\s+/u).length;
        const minimum = Number.parseInt(context.params.minimum, 10);
        const maximum = Number.parseInt(context.params.maximum, 10);

        return {
            success: wordCount >= minimum && wordCount <= maximum
        };
    });
}
